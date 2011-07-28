using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.ViewModels.Organizations
{
    public class EditOrganizationViewModel
    {
        public EditOrganizationViewModel() { }

        public EditOrganizationViewModel(Organization org)
        {
            Id = org.Id;
            Name = org.Name;
            IsEmailDomainRestricted = org.IsEmailDomainRestricted;
            RestrictedDomain = org.EmailDomains.Count > 0 ? org.EmailDomains.First().Domain : string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEmailDomainRestricted { get; set; }
        public string RestrictedDomain { get; set; }
    }
}