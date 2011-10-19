using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Milestones;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.Organization.Controllers
{
    [MJLAuthorize]
    public partial class MilestoneController : MyJobLeadsBaseController
    {
        protected IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel> _milestoneListQuery;
        protected UserByIdQuery _userByIdQuery;

        public MilestoneController(IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel> milestoneListQuery,
                                   UserByIdQuery userByIdQuery)
        {
            _milestoneListQuery = milestoneListQuery;
            _userByIdQuery = userByIdQuery;
        }

        public virtual ActionResult List()
        {
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();
            var milestoneList = _milestoneListQuery.Execute(new ByOrganizationIdParams
            {
                OrganizationId = (int)user.OrganizationId,
                RequestingUserId = user.Id
            });

            return View(milestoneList);
        }
    }
}
