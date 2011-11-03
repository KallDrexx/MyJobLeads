using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using Telerik.Web.Mvc;

namespace MyJobLeads.Areas.Organization.Controllers
{
    [MJLAuthorize]
    public partial class MemberStatsController : MyJobLeadsBaseController
    {
        protected IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel> _memberStatsProcess;
        protected IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> _orgAdminAuthProcess;

        public MemberStatsController(IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel> memberStatsProcess,
                                     IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> orgAdminAuthProcess)
        {
            _memberStatsProcess = memberStatsProcess;
            _orgAdminAuthProcess = orgAdminAuthProcess;
        }

        public virtual ActionResult Index(int organizationId)
        {
            var authResult = _orgAdminAuthProcess.Execute(new OrganizationAdminAuthorizationParams { OrganizationId = organizationId, UserId = CurrentUserId });
            if (!authResult.UserAuthorized)
                RedirectToAction(MVC.Home.Index());

            return View(organizationId);
        }

        public virtual ActionResult GetMemberStats(int organizationId)
        {
            var stats = _memberStatsProcess.Execute(new OrganizationMemberStatisticsParams 
            { 
                OrganizationId = organizationId, 
                RequestingUserId = CurrentUserId 
            });

            return Json(new
            {
                aaData = stats.MemberStats
                              .Select(x => new object[]
                              {
                                  "<img class=\"btnDetails\" src=\"" + Url.Content("~/Content/Images/details_open.png") +"\" />",
                                  Server.HtmlEncode(x.FullName),
                                  Server.HtmlEncode(x.Email),
                                  x.Metrics.NumCompaniesCreated,
                                  x.Metrics.NumContactsCreated,
                                  x.Metrics.NumApplyTasksCreated,
                                  x.Metrics.NumApplyTasksCompleted,
                                  x.Metrics.NumPhoneInterviewTasksCreated,
                                  x.Metrics.NumInPersonInterviewTasksCreated
                              })
                              .ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
