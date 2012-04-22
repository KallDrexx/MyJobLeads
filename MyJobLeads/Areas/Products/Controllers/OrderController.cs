using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.ViewModels.Ordering;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.Ordering;
using MyJobLeads.DomainModel.QueryExtensions;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Exceptions.Ordering;
using MyJobLeads.DomainModel.Entities.FillPerfect;

namespace MyJobLeads.Areas.Products.Controllers
{
    [Authorize]
    public partial class OrderController : MyJobLeadsBaseController
    {
        public OrderController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Confirm(int productId)
        {
            var model = GetConfirmModel(productId);
            if (model == null)
                throw new InvalidOperationException(
                    string.Format("Product id {0} was not found", productId));

            // Set the effective date to today if the last license expired prior to today
            if (model.LicenseEffectiveDate <= DateTime.Today)
                model.LicenseEffectiveDate = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Confirm(OrderConfirmViewModel model)
        {
            if (!model.OrderConfirmed)
            {
                // Regenerate the model
                model = GetConfirmModel(model.ProductId);
                ModelState.AddModelError("OrderConfirmed", "You must check the confirmation box to activate this license");
                return View(model);
            }

            var createdOrder = CreateOrder(model.ProductId);

            if (createdOrder.OrderStatus == OrderStatus.AwaitingPayment)
                throw new NotImplementedException("Pay orders not implemented yet");
            else if (createdOrder.OrderStatus != OrderStatus.Completed)
                throw new InvalidOperationException(
                    string.Format("Order status of {0} is not supported", createdOrder.OrderStatus.ToString()));

            // Order was completed as it was free
            ActivateOrderedLicenses(createdOrder);
            string LicenseType;
            switch (createdOrder.FillPerfectLicenses.First().LicenseType)
            {
                case FillPerfectLicenseType.Trial:
                    LicenseType = "Trial";
                    break;

                case FillPerfectLicenseType.IndividualPaid:
                    LicenseType = "Individual";
                    break;

                case FillPerfectLicenseType.OrganizationGranted:
                    LicenseType = "Organization";
                    break;

                case FillPerfectLicenseType.AdminGranted:
                    LicenseType = "Granted by Administration";
                    break;

                default:
                    LicenseType = "Other";
                    break;
            }

            return RedirectToAction(
                MVC.Products.FillPerfect.LicenseActivated(
                    (Guid)_context.Users.Find(createdOrder.OrderedForId).FillPerfectKey,
                    createdOrder.FillPerfectLicenses.First().EffectiveDate,
                    createdOrder.FillPerfectLicenses.First().ExpirationDate,
                    LicenseType));
        }

        protected OrderConfirmViewModel GetConfirmModel(int productId)
        {
            var model = _context.Products
                                .Where(x => x.Id == productId)
                                .ToList()
                                .Select(x => new OrderConfirmViewModel
                                {
                                    ProductId = x.Id,
                                    Price = x.Price,
                                    LicenseType = x.Type == DomainModel.Enums.ProductType.FillPerfectTrialLicense
                                        ? "Fill Perfect trial" : x.Type == ProductType.FillPerfectIndividualLicense
                                            ? "Fill Perfect personal" : "",
                                    LicenseDurationInWeeks = x.TimeRestricted ? x.DurationInWeeks : 0,
                                    AnotherLicenseAlreadyActive = _context.FpUserLicenses
                                                                          .AsQueryable()
                                                                          .UserActivatedLicenses(CurrentUserId)
                                                                          .Any(y => y.EffectiveDate <= DateTime.Now && y.ExpirationDate >= DateTime.Now),
                                    LicenseEffectiveDate = _context.FpUserLicenses
                                                                   .AsQueryable()
                                                                   .UserLatestFillPerfectActivatedLicenseExpirationDate(CurrentUserId),
                                    MaxPurchaseExceeded = x.MaxPurchaseTimes == 0
                                        ? false
                                        : _context.Orders
                                                  .AsQueryable()
                                                  .UserCompletedOrders(CurrentUserId)
                                                  .Where(y => y.OrderedProducts.Any(z => z.ProductId == x.Id)).Count() >= x.MaxPurchaseTimes
                                })
                                .FirstOrDefault();
            return model;
        }

        protected Order CreateOrder(int productId)
        {
            var product = _context.Products.Where(x => x.Id == productId).FirstOrDefault();
            if (product == null)
                throw new MJLEntityNotFoundException(typeof(Product), productId);

            // Make sure the user hasn't exceeded activation count for this product
            if (_context.Orders
                        .AsQueryable()
                        .UserCompletedOrders(CurrentUserId)
                        .Where(x => x.OrderedProducts.Any(y => y.ProductId == productId))
                        .Count() >= product.MaxPurchaseTimes)
            {
                throw new ProductOrderCountExceededException(CurrentUserId, productId);
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                OrderedById = CurrentUserId,
                OrderedForId = CurrentUserId,
                OrderStatus = OrderStatus.AwaitingPayment,
                TotalPrice = product.Price
            };
            order.OrderedProducts.Add(new OrderedProduct { Product = product, Price = product.Price });

            if (order.TotalPrice == 0)
                order.OrderStatus = OrderStatus.Completed;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        protected void ActivateOrderedLicenses(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (order.OrderStatus != OrderStatus.Completed)
                throw new InvalidOperationException("Cannot activate licenses for an incomplete order");

            // Make sure a license hasn't been activated for this order 
            if (order.FillPerfectLicenses.Count > 0)
                throw new OrderLicensesAlreadyActivatedException(order.Id);

            // Get the next available license activation date
            var startDate = _context.FpUserLicenses.AsQueryable().UserLatestFillPerfectActivatedLicenseExpirationDate(CurrentUserId);
            if (startDate < DateTime.Today)
                startDate = DateTime.Today;

            // Assume only one product per order
            FillPerfectLicenseType licenseType;
            switch (order.OrderedProducts.First().Product.Type)
            {
                case ProductType.FillPerfectTrialLicense:
                    licenseType = FillPerfectLicenseType.Trial;
                    break;

                case ProductType.FillPerfectIndividualLicense:
                    licenseType = FillPerfectLicenseType.IndividualPaid;
                    break;

                case ProductType.FillPerfectOrgLicense:
                    licenseType = FillPerfectLicenseType.OrganizationGranted;
                    break;

                default:
                    licenseType = FillPerfectLicenseType.None;
                    break;
            }

            order.FillPerfectLicenses.Add(new FpUserLicense
            {
                EffectiveDate = startDate,
                ExpirationDate = startDate.AddDays((order.OrderedProducts.First().Product.DurationInWeeks * 7) + 1),
                LicenseType = licenseType
            });

            // If the user doesn't have a FP key, assign them one
            var user = _context.Users.Where(x => x.Id == order.OrderedForId).FirstOrDefault();
            if (user.FillPerfectKey == null)
                user.FillPerfectKey = Guid.NewGuid();

            _context.SaveChanges();
        }
    }
}
