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
using MyJobLeads.DomainModel.ProcessParams.Milestones;
using MyJobLeads.ViewModels.Milestones;
using AutoMapper;

namespace MyJobLeads.Areas.Organization.Controllers
{
    [MJLAuthorize]
    public partial class MilestoneController : MyJobLeadsBaseController
    {
        protected IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel> _milestoneListQuery;
        protected IProcess<MilestoneIdParams, MilestoneDisplayViewModel> _milestoneQuery;
        IProcess<SaveMilestoneParams, MilestoneIdViewModel> _saveMilestoneProcess;
        protected UserByIdQuery _userByIdQuery;

        public MilestoneController(IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel> milestoneListQuery,
                                          IProcess<MilestoneIdParams, MilestoneDisplayViewModel> milestoneQuery,
                                          IProcess<SaveMilestoneParams, MilestoneIdViewModel> saveMilestoneProcess,
                                          UserByIdQuery userByIdQuery)
        {
            _milestoneListQuery = milestoneListQuery;
            _userByIdQuery = userByIdQuery;
            _milestoneQuery = milestoneQuery;
            _saveMilestoneProcess = saveMilestoneProcess;
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

        public virtual ActionResult Details(int id)
        {
            var model = _milestoneQuery.Execute(new MilestoneIdParams { MilestoneId = id, RequestingUserId = CurrentUserId });
            return View(model);
        }

        public virtual ActionResult Create()
        {
            var model = new EditMilestoneViewModel();

            // Get a list of all milestones for the organization
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();
            var msList = _milestoneListQuery.Execute(new ByOrganizationIdParams { OrganizationId = user.Organization.Id, RequestingUserId = user.Id });
            model.SetAvailableMilestoneList(msList);

            return View(MVC.Organization.Milestone.Views.Update, model);
        }

        public virtual ActionResult Update(int id)
        {
            // Get the milestone
            var milestone = _milestoneQuery.Execute(new MilestoneIdParams { RequestingUserId = CurrentUserId, MilestoneId = id });
            var model = Mapper.Map<MilestoneDisplayViewModel, EditMilestoneViewModel>(milestone);

            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();
            var msList = _milestoneListQuery.Execute(new ByOrganizationIdParams { OrganizationId = user.Organization.Id, RequestingUserId = user.Id });
            model.SetAvailableMilestoneList(msList);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Update(EditMilestoneViewModel model)
        {
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();

            if (ModelState.IsValid)
            {
                var saveParams = Mapper.Map<EditMilestoneViewModel, SaveMilestoneParams>(model);
                saveParams.RequestingUserId = CurrentUserId;
                saveParams.OrganizationId = user.Organization.Id;
                var result = _saveMilestoneProcess.Execute(saveParams);

                return RedirectToAction(MVC.Organization.Milestone.Details(result.Id));
            }

            // Not valid
            var msList = _milestoneListQuery.Execute(new ByOrganizationIdParams { OrganizationId = user.Organization.Id, RequestingUserId = user.Id });
            model.SetAvailableMilestoneList(msList);
            return View(model);
        }
    }
}
