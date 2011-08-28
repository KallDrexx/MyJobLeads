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

namespace MyJobLeads.Controllers
{
    public partial class PositionController : MyJobLeadsBaseController
    {
        protected IProcess<CreatePositionParams, PositionDisplayViewModel> _createProcess;
        protected IProcess<EditPositionParams, PositionDisplayViewModel> _editProcess;
        protected PositionByIdQuery _posByIdQuery;
        protected CompanyByIdQuery _companyByIdQuery;

        public PositionController(MyJobLeadsDbContext context,
                                    IProcess<CreatePositionParams, PositionDisplayViewModel> createProcess,
                                    IProcess<EditPositionParams, PositionDisplayViewModel> editProcess,
                                    PositionByIdQuery posByIdQuery,
                                    CompanyByIdQuery compByIdQuery)
        {
            _context = context;
            _createProcess = createProcess;
            _editProcess = editProcess;
            _posByIdQuery = posByIdQuery;
            _companyByIdQuery = compByIdQuery;
        }

        public virtual ActionResult Details(int id)
        {
            var position = _posByIdQuery.WithPositionId(id).Execute();
            var model = Mapper.Map<Position, PositionDisplayViewModel>(position);
            return View(model);
        }

        public virtual ActionResult Add(int companyId)
        {
            var company = _companyByIdQuery.WithCompanyId(companyId).Execute();
            var model = new EditPositionViewModel
            {
                Company = Mapper.Map<Company, CompanySummaryViewModel>(company)
            };

            return View("Edit", model);
        }

        public virtual ActionResult Edit(int id)
        {
            var position = _posByIdQuery.WithPositionId(id).Execute();
            var model = Mapper.Map<Position, EditPositionViewModel>(position);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(EditPositionViewModel model)
        {
            PositionDisplayViewModel editedModel;

            if (model.Id == 0)
                editedModel = _createProcess.Execute(Mapper.Map<EditPositionViewModel, CreatePositionParams>(model));
            else
                editedModel = _editProcess.Execute(Mapper.Map<EditPositionViewModel, EditPositionParams>(model));

            return RedirectToAction(MVC.Position.Details(editedModel.Id));
        }
    }
}
