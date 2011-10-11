using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Organizations;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Organizations;

namespace MyJobLeads.Areas.Organization.Controllers
{
    [MJLAuthorize]
    public partial class DashboardController : MyJobLeadsBaseController
    {
        protected IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel> _orgDocVisibilityProcess;

        public DashboardController(IServiceFactory factory, 
                                   IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel> orgDocVisibilityProcess)
        {
            _serviceFactory = factory;
            _unitOfWork = factory.GetService<IUnitOfWork>();
            _orgDocVisibilityProcess = orgDocVisibilityProcess;
        }

        public virtual ActionResult Index()
        {
            var org = _serviceFactory.GetService<OrganizationByAdministeringUserQuery>()
                                     .Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = CurrentUserId });

            if (org == null)
                return RedirectToAction(MVC.Home.Index());

            return View(org);
        }

        public virtual ActionResult HideDocumentFromMembers(int docId)
        {
            _orgDocVisibilityProcess.Execute(new OrgMemberDocVisibilityByOrgAdminParams
            {
                RequestingUserId = CurrentUserId,
                OfficialDocumentId = docId,
                ShowToMembers = false
            });

            return RedirectToAction(MVC.Organization.Dashboard.Index());
        }

        public virtual ActionResult ShowDocumentToMembers(int docId)
        {
            _orgDocVisibilityProcess.Execute(new OrgMemberDocVisibilityByOrgAdminParams
            {
                RequestingUserId = CurrentUserId,
                OfficialDocumentId = docId,
                ShowToMembers = true
            });

            return RedirectToAction(MVC.Organization.Dashboard.Index());
        }
    }
}
