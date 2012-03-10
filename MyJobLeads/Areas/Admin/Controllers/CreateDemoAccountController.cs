using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Areas.Admin.Models.DemoAccountCreation;
using System.Transactions;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ViewModels.Admin;
using MyJobLeads.ViewModels.Admin;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ProcessParams.Admin;

namespace MyJobLeads.Areas.Admin.Controllers
{
    [RequiresSiteAdmin]
    public partial class CreateDemoAccountController : MyJobLeadsBaseController
    {
        protected IProcess<CreateDemoAccountParams, CreatedDemoAccountViewModel> _createDemoActProc;

        public CreateDemoAccountController(MyJobLeadsDbContext context, 
                                            IProcess<CreateDemoAccountParams, CreatedDemoAccountViewModel> createDemoActProc)
        {
            _context = context;
            _createDemoActProc = createDemoActProc;
        }

        public virtual ActionResult Index()
        {
            return View(new CreateDemoAccountViewModel());
        }

        [HttpPost]
        public virtual ActionResult Index(CreateDemoAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _createDemoActProc.Execute(new CreateDemoAccountParams
            {
                RequestingUserID = CurrentUserId,
                Name = model.Name,
                Email = model.Email,
                OrganizationName = model.OrganizationName,
                Password = model.Password
            });

            return RedirectToAction(MVC.Admin.CreateDemoAccount.Success(model.Name, model.Email, model.Password));
        }

        public virtual ActionResult Success(string name, string email, string password)
        {
            return View(new CreateDemoAccountSuccessViewModel
            {
                Name = name,
                Email = email,
                Password = password
            });
        }
    }
}
