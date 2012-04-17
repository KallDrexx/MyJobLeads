using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.ViewModels.Ordering;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.Areas.Products.Controllers
{
    [Authorize]
    public class OrderController : MyJobLeadsBaseController
    {
        public OrderController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public ActionResult Confirm(int productId)
        {
            var model = _context.Products
                                .Where(x => x.Id == productId)
                                .Select(x => new OrderConfirmViewModel
                                {
                                    ProductId = x.Id,
                                    Price = x.Price,
                                    LicenseType = x.Type == DomainModel.Enums.ProductType.FillPerfectTrialLicense
                                        ? "Fill Perfect trial" : x.Type == ProductType.FillPerfectIndividualLicense
                                            ? "Fill Perfect personal" : "",
                                    LicenseDurationInWeeks = x.TimeRestricted ? x.DurationInWeeks : 0,
                                    AnotherLicenseAlreadyActive = _context.FpUserLicenses.Any(y => y.EffectiveDate <= DateTime.Now && y.ExpirationDate >= DateTime.Now),
                                    LicenseEffectiveDate = _context.FpUserLicenses.Select(y => y.ExpirationDate).DefaultIfEmpty().Max(),
                                    MaxPurchaseExceeded = x.MaxPurchaseTimes == 0
                                        ? false
                                        : _context.Orders.Where(y => y.OrderStatusValue == (int)OrderStatus.Paid && y.OrderedProducts.Any(z => z.ProductId == x.Id)).Count() >= x.MaxPurchaseTimes
                                })
                                .FirstOrDefault();
            if (model == null)
                throw new InvalidOperationException(
                    string.Format("Product id {0} was not found", productId));

            return View(model);
        }
    }
}
