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
        protected const string IGNORE_USERS_COOKIE_NAME = "SiteActivityIgnoredUsers";

        protected IProcess<ExportSiteActivityReportParams, FileContentViewModel> _exportActivityProc;

        public SiteActivityController(MyJobLeadsDbContext context, IProcess<ExportSiteActivityReportParams, FileContentViewModel> exportActivityProc)
        {
            _context = context;
            _exportActivityProc = exportActivityProc;
        }

        public virtual ActionResult Index()
        {
            var model = new ExportSiteActivityViewModel();

            if (Request.Cookies[IGNORE_USERS_COOKIE_NAME] != null)
                model.IgnoredUserList = HttpUtility.UrlDecode(Request.Cookies[IGNORE_USERS_COOKIE_NAME].Value);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(ExportSiteActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Set a cookie with the 
            Response.Cookies.Add(new HttpCookie(IGNORE_USERS_COOKIE_NAME, model.IgnoredUserList));

            var content = _exportActivityProc.Execute(new ExportSiteActivityReportParams
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                SortDateDescending = model.SortByDescending,
                IgnoredUsers = model.IgnoredUserList
                                    .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.Trim())
                                    .ToList()
            });

            return File(content.ExportFileContents, content.Mimetype, content.FileName);
        }
    }
}
