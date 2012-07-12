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
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.Areas.Products.Models;

namespace MyJobLeads.Areas.Products.Controllers
{
    [MJLAuthorize]
    public partial class OrderController : MyJobLeadsBaseController
    {
        public OrderController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Confirm(int productId)
        {
            var orderUtils = new OrderUtils(_context, CurrentUserId);
            var model = orderUtils.GetConfirmModel(productId);
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
            var orderUtils = new OrderUtils(_context, CurrentUserId);

            if (!model.OrderConfirmed)
            {
                // Regenerate the model
                model = orderUtils.GetConfirmModel(model.ProductId);
                ModelState.AddModelError("OrderConfirmed", "You must check the confirmation box to activate this license");
                return View(model);
            }

            var createdOrder = orderUtils.CreateOrder(model.ProductId);
            if (createdOrder.OrderStatus == OrderStatus.AwaitingPayment)
                return RedirectToAction(MVC.Products.Pay.Index(createdOrder.Id));

            else if (createdOrder.OrderStatus != OrderStatus.Completed)
                throw new InvalidOperationException(
                    string.Format("Order status of {0} is not supported", createdOrder.OrderStatus.ToString()));

            // Order was completed as it was free
            orderUtils.ActivateOrderedLicenses(createdOrder);
            string LicenseType = orderUtils.GetLicenseDescription(createdOrder.FillPerfectLicenses.First().LicenseType);
           
            return RedirectToAction(
                MVC.Products.FillPerfect.LicenseActivated(
                    (Guid)_context.Users.Find(createdOrder.OrderedForId).FillPerfectKey,
                    createdOrder.FillPerfectLicenses.First().EffectiveDate,
                    createdOrder.FillPerfectLicenses.First().ExpirationDate,
                    LicenseType));
        }

        public virtual ActionResult ConfirmFillPerfectOrgOrder()
        {
            var orderUtils = new OrderUtils(_context, CurrentUserId);
            var model = orderUtils.GetFpOrgConfirmModel();
            if (model == null)
                throw new InvalidOperationException("Organization does not have a FP license");

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult ConfirmFillPerfectOrgOrder(FillPerfectOrgLicenseConfirmViewModel model)
        {
            var orderUtils = new OrderUtils(_context, CurrentUserId);

            if (!model.OrderConfirmed)
            {
                // Regenerate the model
                model = orderUtils.GetFpOrgConfirmModel();
                ModelState.AddModelError("OrderConfirmed", "You must check the confirmation box to activate this license");
                return View(model);
            }

            // Regenerate the model for license details
            model = orderUtils.GetFpOrgConfirmModel();

            // Create the order and automatically activate the license
            var order = orderUtils.CreateOrder();
            order.FillPerfectLicenses.Add(new FpUserLicense
            {
                LicenseType = FillPerfectLicenseType.OrganizationGranted,
                EffectiveDate = model.ActivatedLicenseExpiratioDate >= model.EffectiveLicenseDate ? model.ActivatedLicenseExpiratioDate : model.EffectiveLicenseDate,
                ExpirationDate = model.ExpirationDate
            });
            _context.SaveChanges();

            // Show the user confirmation
            string orgName = _context.Users.Where(x => x.Id == CurrentUserId).Select(x => x.Organization.Name).FirstOrDefault();

            return RedirectToAction(
                MVC.Products.FillPerfect.LicenseActivated(
                    (Guid)_context.Users.Find(order.OrderedForId).FillPerfectKey,
                    order.FillPerfectLicenses.First().EffectiveDate,
                    order.FillPerfectLicenses.First().ExpirationDate,
                    "License granted by " + orgName));
        }
    }
}
