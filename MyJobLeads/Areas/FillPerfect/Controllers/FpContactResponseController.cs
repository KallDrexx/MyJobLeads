using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    public class FpContactResponseController : MyJobLeadsBaseController
    {
        protected IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> _fetchResponseProc;

        public FpContactResponseController(MyJobLeadsDbContext context, IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> fetchResponseProc)
        {
            _context = context;
            _fetchResponseProc = fetchResponseProc;
        }

        public ActionResult Index()
        {
            _fetchResponseProc.Execute(new FetchFpContactResponseEmailsParams());
            return View();
        }
    }
}
