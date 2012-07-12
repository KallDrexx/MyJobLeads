using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.ViewModels.Ordering;
using MyJobLeads.DomainModel.Entities.Ordering;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.QueryExtensions;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Exceptions.Ordering;

namespace MyJobLeads.Areas.Products.Models
{
    public class OrderUtils
    {
        protected MyJobLeadsDbContext _context;
        protected int _currentUserId;

        public OrderUtils(MyJobLeadsDbContext context, int currentUserId)
        {
            _context = context;
            _currentUserId = currentUserId;
        }

        public OrderConfirmViewModel GetConfirmModel(int productId)
        {
            var model = _context.Products
                                .Where(x => x.Id == productId)
                                .ToList()
                                .Select(x => new OrderConfirmViewModel
                                {
                                    ProductId = x.Id,
                                    Price = x.Price,
                                    LicenseType = x.Type == DomainModel.Enums.ProductType.FillPerfectTrialLicense
                                        ? "FillPerfect trial" : x.Type == ProductType.FillPerfectIndividualLicense
                                            ? "FillPerfect personal" : "",
                                    LicenseDurationInWeeks = x.TimeRestricted ? x.DurationInWeeks : 0,
                                    AnotherLicenseAlreadyActive = _context.FpUserLicenses
                                                                          .AsQueryable()
                                                                          .UserActivatedLicenses(_currentUserId)
                                                                          .Any(y => y.EffectiveDate <= DateTime.Now && y.ExpirationDate >= DateTime.Now),
                                    LicenseEffectiveDate = _context.FpUserLicenses
                                                                   .AsQueryable()
                                                                   .UserLatestFillPerfectActivatedLicenseExpirationDate(_currentUserId),
                                    MaxPurchaseExceeded = x.MaxPurchaseTimes == 0
                                        ? false
                                        : _context.Orders
                                                  .AsQueryable()
                                                  .UserCompletedOrders(_currentUserId)
                                                  .Where(y => y.OrderedProducts.Any(z => z.ProductId == x.Id)).Count() >= x.MaxPurchaseTimes
                                })
                                .FirstOrDefault();
            return model;
        }

        public FillPerfectOrgLicenseConfirmViewModel GetFpOrgConfirmModel()
        {
            return _context.Users
                           .Where(x => x.Id == _currentUserId)
                           .SelectMany(x => x.Organization.FillPerfectLicenses)
                           .Where(x => x.EffectiveDate <= DateTime.Today && x.ExpirationDate >= DateTime.Now)
                           .ToList()
                           .Select(x => new FillPerfectOrgLicenseConfirmViewModel
                           {
                               EffectiveLicenseDate = x.EffectiveDate,
                               ExpirationDate = x.ExpirationDate,
                               ActivatedLicenseExpiratioDate = _context.FpUserLicenses
                                                                       .AsQueryable()
                                                                       .UserLatestFillPerfectActivatedLicenseExpirationDate(_currentUserId),
                               OrganizationName = x.Organization.Name
                           })
                           .FirstOrDefault();
        }

        public Order CreateOrder(int productId = 0)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                OrderedById = _currentUserId,
                OrderedForId = _currentUserId,
                OrderStatus = OrderStatus.AwaitingPayment
            };

            if (productId > 0)
            {
                var product = _context.Products.Where(x => x.Id == productId).FirstOrDefault();
                if (product == null)
                    throw new MJLEntityNotFoundException(typeof(Product), productId);

                // Make sure the user hasn't exceeded activation count for this product
                int timesActivated = _context.Orders
                                            .AsQueryable()
                                            .UserCompletedOrders(_currentUserId)
                                            .Where(x => x.OrderedProducts.Any(y => y.ProductId == productId))
                                            .Count();

                if (product.MaxPurchaseTimes > 0 && timesActivated >= product.MaxPurchaseTimes)
                {
                    throw new ProductOrderCountExceededException(_currentUserId, productId);
                }

                order.TotalPrice = product.Price;
                order.OrderedProducts.Add(new OrderedProduct { Product = product, Price = product.Price });
            }

            if (order.TotalPrice == 0)
                order.OrderStatus = OrderStatus.Completed;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public void ActivateOrderedLicenses(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (order.OrderStatus != OrderStatus.Completed)
                throw new InvalidOperationException("Cannot activate licenses for an incomplete order");

            // Make sure a license hasn't been activated for this order 
            if (order.FillPerfectLicenses.Count > 0)
                throw new OrderLicensesAlreadyActivatedException(order.Id);

            // Get the next available license activation date
            var startDate = _context.FpUserLicenses.AsQueryable().UserLatestFillPerfectActivatedLicenseExpirationDate(_currentUserId);
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

        public string GetLicenseDescription(FillPerfectLicenseType licenseType)
        {
            switch (licenseType)
            {
                case FillPerfectLicenseType.Trial:
                    return "Trial";

                case FillPerfectLicenseType.IndividualPaid:
                    return "Individual";

                case FillPerfectLicenseType.OrganizationGranted:
                    return "Organization";

                case FillPerfectLicenseType.AdminGranted:
                    return "Granted by Administration";

                default:
                    return "Other";
            }
        }
    }
}