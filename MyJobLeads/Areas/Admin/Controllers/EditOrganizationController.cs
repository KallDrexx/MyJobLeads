using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using AutoMapper;
using MyJobLeads.Areas.Admin.Models;

namespace MyJobLeads.Areas.Admin.Controllers
{
    [RequiresSiteAdmin]
    public partial class EditOrganizationController : MyJobLeadsBaseController
    {
        protected IProcess<CreateOrgWithAdminUserParams, GeneralSuccessResultViewModel> _createOrgProc;

        public EditOrganizationController(IProcess<CreateOrgWithAdminUserParams, GeneralSuccessResultViewModel> createOrgProc)
        {
            _createOrgProc = createOrgProc;
        }

        public virtual ActionResult CreateOrganization()
        {
            return View(new CreateOrganizationWithAdminViewModel());
        }

        [HttpPost]
        public virtual ActionResult CreateOrganization(CreateOrganizationWithAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parameters = Mapper.Map<CreateOrganizationWithAdminViewModel, CreateOrgWithAdminUserParams>(model);
                parameters.RequestingUserId = CurrentUserId;

                var result = _createOrgProc.Execute(parameters);
                return View(MVC.Admin.EditOrganization.Views.CreateOrgSuccessful, model);
            }

            return View(model);
        }
    }
}
