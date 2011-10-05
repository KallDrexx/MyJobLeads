using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.ViewModels.Companies;

namespace MyJobLeads.ViewModels.Contacts
{
    public class EditContactViewModel
    {
        public EditContactViewModel() { }

        public EditContactViewModel(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException("contact");

            Id = contact.Id;
            Name = contact.Name;
            Title = contact.Title;
            DirectPhone = contact.DirectPhone;
            MobilePhone = contact.MobilePhone;
            Extension = contact.Extension;
            Email = contact.Email;
            Assistant = contact.Assistant;
            ReferredBy = contact.ReferredBy;
            Notes = contact.Notes;

            Company = new CompanySummaryViewModel(contact.Company);
        }

        public EditContactViewModel(Company company)
        {
            if (company == null)
                throw new ArgumentNullException("company");

            Company = new CompanySummaryViewModel(company);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string DirectPhone { get; set; }
        public string MobilePhone { get; set; }
        public string Extension { get; set; }
        public string Email { get; set; }
        public string Assistant { get; set; }
        public string ReferredBy { get; set; }
        public string Notes { get; set; }

        public CompanySummaryViewModel Company { get; set; }
    }
}
