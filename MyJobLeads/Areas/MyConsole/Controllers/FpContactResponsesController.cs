using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.MyConsole.Controllers
{
    [RequiresSiteAdmin]
    public class FpContactResponsesController : MyJobLeadsBaseController
    {
        protected IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> _fetchResponseProc;

        public FpContactResponsesController(MyJobLeadsDbContext context, IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> fetchResponseProc)
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
