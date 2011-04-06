using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Commands.Contacts;

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
            ViewBag.CompanyId = companyId;
            return View(MVC.Contact.Views.Edit, new Contact());
        }

        public virtual ActionResult Edit(int id)
        {
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
            ViewBag.CompanyId = contact.Company.Id;
            return View(contact);
        }

        [HttpPost]
        public virtual ActionResult Edit(Contact contact, int companyId)
        {
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
