using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Areas.Reports.Models.SiteActivityViewModels;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.ViewModels.General;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.Reports.Controllers
{
    [RequiresSiteAdmin]
    public partial class SiteActivityController : MyJobLeadsBaseController
    {
        protected IProcess<ExportSiteActivityReportParams, FileContentViewModel> _exportActivityProc;

        public SiteActivityController(MyJobLeadsDbContext context, IProcess<ExportSiteActivityReportParams, FileContentViewModel> exportActivityProc)
        {
            _context = context;
            _exportActivityProc = exportActivityProc;
        }

        public virtual ActionResult Index()
        {
            return View(new ExportSiteActivityViewModel());
        }

        [HttpPost]
        public virtual ActionResult Index(ExportSiteActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var content = _exportActivityProc.Execute(new ExportSiteActivityReportParams
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate
            });

            return File(content.ExportFileContents, content.Mimetype, content.FileName);
        }
    }
}
