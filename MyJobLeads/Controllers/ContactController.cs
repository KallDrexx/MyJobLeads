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

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class ContactController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;

        public ContactController(IServiceFactory factory)
        {
            _serviceFactory = factory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
        }

        public virtual ActionResult Add(int companyId)
        {
            var company = new CompanyByIdQuery(_unitOfWork).WithCompanyId(companyId).Execute();
            var model = new EditContactViewModel(company);
            return View(MVC.Contact.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
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
                model.Company = new CompanySummaryViewModel(new CompanyByIdQuery(_unitOfWork).WithCompanyId(model.Company.Id).Execute());
                return View(model);
            }
        }

        public virtual ActionResult Details(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
            return View(contact);
        }

        public virtual ActionResult GetSummary(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
            if (contact == null)
                return new EmptyResult();

            return PartialView(MVC.Contact.Views._ContactSidebarDisplay, contact);
        }
    }
}
