using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.FillPerfect;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;
using MyJobLeads.Areas.Products.Models;

namespace MyJobLeads.Areas.Products.Controllers
{
    [Authorize]
    public partial class FillPerfectController : MyJobLeadsBaseController
    {
        protected IProcess<GetOrderableFillPerfectLicensesParams, FpLicensesAvailableForOrderingViewModel> _getFpLicensesProc;

        public FillPerfectController(MyJobLeadsDbContext context, 
                IProcess<GetOrderableFillPerfectLicensesParams, FpLicensesAvailableForOrderingViewModel> getFpLicensesProc)
        {
            _context = context;
            _getFpLicensesProc = getFpLicensesProc;
        }

        public virtual ActionResult Index()
        {
            var model = _getFpLicensesProc.Execute(new GetOrderableFillPerfectLicensesParams { RequestingUserID = CurrentUserId });
            return View(model);
        }

        public virtual ActionResult LicenseActivated(Guid fpKey, DateTime LicenseStart, DateTime LicenseEnd, string LicenseType)
        {
            var model = new ActivatedFillPerfectLicenseViewModel
            {
                UserFpKey = fpKey,
                LicenseEffectiveDate = LicenseStart,
                LicenseExpirationDate = LicenseEnd,
                LicenseType = LicenseType
            };
            return View(model);
        }
    }
}
