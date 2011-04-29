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

namespace MyJobLeads.Controllers
{
    public partial class ContactController : MyJobLeadsBaseController
    {
        public ContactController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual ActionResult Add(int companyId)
        {
            var model = new EditContactViewModel { Contact = new Contact() };
            model.Company = new CompanyByIdQuery(_unitOfWork).WithCompanyId(companyId).Execute();
            if (model.Company == null)
                throw new MJLEntityNotFoundException(typeof(Company), companyId);

            return View(MVC.Contact.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
            ViewBag.CompanyId = contact.Company.Id;
            return View(new EditContactViewModel { Contact = contact, Company = contact.Company });
        }

        [HttpPost]
        public virtual ActionResult Edit(EditContactViewModel contactModel, int companyId)
        {
            var contact = contactModel.Contact;

            // Determine if this is a new contact or not
            if (contact.Id == 0)
            {
                contact = new CreateContactCommand(_unitOfWork).WithCompanyId(companyId)
                                                               .SetAssistant(contact.Assistant)
                                                               .SetDirectPhone(contact.DirectPhone)
                                                               .SetEmail(contact.Email)
                                                               .SetExtension(contact.Extension)
                                                               .SetMobilePhone(contact.MobilePhone)
                                                               .SetName(contact.Name)
                                                               .SetNotes(contact.Notes)
                                                               .SetReferredBy(contact.ReferredBy)
                                                               .RequestedByUserId(CurrentUserId)
                                                               .Execute();
            }
            else
            {
                contact = new EditContactCommand(_unitOfWork).WithContactId(contact.Id)
                                                             .SetAssistant(contact.Assistant)
                                                             .SetDirectPhone(contact.DirectPhone)
                                                             .SetEmail(contact.Email)
                                                             .SetExtension(contact.Extension)
                                                             .SetMobilePhone(contact.MobilePhone)
                                                             .SetName(contact.Name)
                                                             .SetNotes(contact.Notes)
                                                             .SetReferredBy(contact.ReferredBy)
                                                             .RequestedByUserId(CurrentUserId)
                                                             .Execute();
            }

            return RedirectToAction(MVC.Contact.Details(contact.Id));
        }

        public virtual ActionResult Details(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
            return View(contact);
        }
    }
}
