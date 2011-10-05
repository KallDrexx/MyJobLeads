using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.ViewModels.Contacts;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using FluentValidation;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class ContactController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;
        protected IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> _companyAuthProcess;
        protected IProcess<ContactAutorizationParams, AuthorizationResultViewModel> _contactAuthProcess;

        public ContactController(IServiceFactory factory, 
                                    IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> compAuthProcess,
                                    IProcess<ContactAutorizationParams, AuthorizationResultViewModel> contactAuthProcess)
        {
            _serviceFactory = factory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            _companyAuthProcess = compAuthProcess;
            _contactAuthProcess = contactAuthProcess;
        }

        public virtual ActionResult Add(int companyId)
        {
            var company = new CompanyByIdQuery(_unitOfWork, _companyAuthProcess).WithCompanyId(companyId)
                                                                                .RequestedByUserId(CurrentUserId)
                                                                                .Execute();
            if (company == null)
            {
                ViewBag.EntityType = "Company";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = new EditContactViewModel(company);
            return View(MVC.Contact.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork, _contactAuthProcess).RequestedByUserId(CurrentUserId).WithContactId(id).Execute();
            if (contact == null)
            {
                ViewBag.EntityType = "Contact";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = new EditContactViewModel(contact);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(EditContactViewModel model)
        {
            Contact contact;

            try
            {
                // Determine if this is a new contact or not
                if (model.Id == 0)
                {
                    contact = new CreateContactCommand(_serviceFactory).WithCompanyId(model.Company.Id)
                                                                   .SetAssistant(model.Assistant)
                                                                   .SetDirectPhone(model.DirectPhone)
                                                                   .SetEmail(model.Email)
                                                                   .SetExtension(model.Extension)
                                                                   .SetMobilePhone(model.MobilePhone)
                                                                   .SetName(model.Name)
                                                                   .SetNotes(model.Notes)
                                                                   .SetTitle(model.Title)
                                                                   .SetReferredBy(model.ReferredBy)
                                                                   .RequestedByUserId(CurrentUserId)
                                                                   .Execute();
                }
                else
                {
                    contact = new EditContactCommand(_serviceFactory).WithContactId(model.Id)
                                                                 .SetAssistant(model.Assistant)
                                                                 .SetDirectPhone(model.DirectPhone)
                                                                 .SetEmail(model.Email)
                                                                 .SetExtension(model.Extension)
                                                                 .SetMobilePhone(model.MobilePhone)
                                                                 .SetName(model.Name)
                                                                 .SetNotes(model.Notes)
                                                                 .SetTitle(model.Title)
                                                                 .SetReferredBy(model.ReferredBy)
                                                                 .RequestedByUserId(CurrentUserId)
                                                                 .Execute();
                }

                return RedirectToAction(MVC.Contact.Details(contact.Id));
            }

            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                // Retrieve the company from the database for the sidebar
                model.Company = new CompanySummaryViewModel(new CompanyByIdQuery(_unitOfWork, _companyAuthProcess).WithCompanyId(model.Company.Id).Execute());
                return View(model);
            }
        }

        public virtual ActionResult Details(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork, _contactAuthProcess).WithContactId(id).RequestedByUserId(CurrentUserId).Execute();
            if (contact == null)
            {
                ViewBag.EntityType = "Contact";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = new ContactDisplayViewModel(contact);
            return View(model);
        }
    }
}
