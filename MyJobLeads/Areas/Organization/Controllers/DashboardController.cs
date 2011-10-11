using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Organizations;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.Organization.Controllers
{
    [MJLAuthorize]
    public partial class DashboardController : MyJobLeadsBaseController
    {
        public DashboardController(IServiceFactory factory)
        {
            _serviceFactory = factory;
            _unitOfWork = factory.GetService<IUnitOfWork>();
        }

        public virtual ActionResult Index()
        {
            var org = _serviceFactory.GetService<OrganizationByAdministeringUserQuery>()
                                     .Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = CurrentUserId });

            if (org == null)
                return RedirectToAction(MVC.Home.Index());

            return View(org);
        }
    }
}
