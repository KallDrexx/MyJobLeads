using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.FillPerfect;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;

namespace MyJobLeads.Areas.Products.Controllers
{
    [Authorize]
    public class FillPerfectController : MyJobLeadsBaseController
    {
        protected IProcess<GetOrderableFillPerfectLicensesParams, FpLicensesAvailableForOrderingViewModel> _getFpLicensesProc;

        public FillPerfectController(MyJobLeadsDbContext context, 
                IProcess<GetOrderableFillPerfectLicensesParams, FpLicensesAvailableForOrderingViewModel> getFpLicensesProc)
        {
            _context = context;
            _getFpLicensesProc = getFpLicensesProc;
        }

        public ActionResult Index()
        {
            var model = _getFpLicensesProc.Execute(new GetOrderableFillPerfectLicensesParams { RequestingUserID = CurrentUserId });
            return View(model);
        }

    }
}
