﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.ViewModels.Contacts
{
    public class ContactSummaryViewModel
    {
        public ContactSummaryViewModel() { }

        public ContactSummaryViewModel(Contact contact)
        {
            if (contact == null)
                return;

            Id = contact.Id;
            Name = contact.Name;
            Title = contact.Title;
            MobilePhone = contact.MobilePhone;
            Email = contact.Email;
            Assistant = contact.Assistant;
            ReferredBy = contact.ReferredBy;
            Notes = contact.Notes;

            DirectPhoneWithExtension = string.IsNullOrWhiteSpace(contact.Extension)
                                       ? contact.DirectPhone
                                       : string.Format("{0} Ext: {1}", contact.DirectPhone, contact.Extension);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string DirectPhoneWithExtension { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Assistant { get; set; }
        public string ReferredBy { get; set; }
        public string Notes { get; set; }
    }
}