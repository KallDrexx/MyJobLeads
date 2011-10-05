using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Queries.Positions;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.ViewModels.Positions;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class PositionController : MyJobLeadsBaseController
    {
        protected IProcess<CreatePositionParams, PositionDisplayViewModel> _createProcess;
        protected IProcess<EditPositionParams, PositionDisplayViewModel> _editProcess;
        protected PositionByIdQuery _posByIdQuery;
        protected CompanyByIdQuery _companyByIdQuery;
        protected IProcess<GetPositionListForUserParams, PositionListViewModel> _getPositionListProcess;

        public PositionController(MyJobLeadsDbContext context,
                                    IProcess<CreatePositionParams, PositionDisplayViewModel> createProcess,
                                    IProcess<EditPositionParams, PositionDisplayViewModel> editProcess,
                                    IProcess<GetPositionListForUserParams, PositionListViewModel> getPositionListProcess,
                                    PositionByIdQuery posByIdQuery,
                                    CompanyByIdQuery compByIdQuery)
        {
            _context = context;
            _createProcess = createProcess;
            _editProcess = editProcess;
            _posByIdQuery = posByIdQuery;
            _companyByIdQuery = compByIdQuery;
            _getPositionListProcess = getPositionListProcess;
        }

        public virtual ActionResult List()
        {
            var positions = _getPositionListProcess.Execute(new GetPositionListForUserParams { UserId = CurrentUserId });
            return View(positions);
        }

        public virtual ActionResult Details(int id)
        {
            var position = _posByIdQuery.WithPositionId(id).RequestedByUserId(CurrentUserId).Execute();
            if (position == null)
            {
                ViewBag.EntityType = "Position";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = Mapper.Map<Position, PositionDisplayViewModel>(position);
            return View(model);
        }

        public virtual ActionResult Add(int companyId)
        {
            var company = _companyByIdQuery.WithCompanyId(companyId).RequestedByUserId(CurrentUserId).Execute();
            if (company == null)
            {
                ViewBag.EntityType = "Position";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = new EditPositionViewModel
            {
                Company = Mapper.Map<Company, CompanySummaryViewModel>(company)
            };

            return View(MVC.Position.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var position = _posByIdQuery.WithPositionId(id).RequestedByUserId(CurrentUserId).Execute();
            if (position == null)
            {
                ViewBag.EntityType = "Position";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = Mapper.Map<Position, EditPositionViewModel>(position);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(EditPositionViewModel model)
        {
            PositionDisplayViewModel editedModel;
            model.RequestedUserId = CurrentUserId;

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                model.Company = new CompanySummaryViewModel(_companyByIdQuery.WithCompanyId(model.Company.Id).Execute());
                return View(model);
            }

            try
            {
                if (model.Id == 0)
                    editedModel = _createProcess.Execute(Mapper.Map<EditPositionViewModel, CreatePositionParams>(model));
                else
                    editedModel = _editProcess.Execute(Mapper.Map<EditPositionViewModel, EditPositionParams>(model));
            }
            catch (UserNotAuthorizedForEntityException ex)
            {
                ViewBag.EntityType = ex.EntityType.Name;
                return View(MVC.Shared.Views.EntityNotFound);
            }

            return RedirectToAction(MVC.Position.Details(editedModel.Id));
        }
    }
}
