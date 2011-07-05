﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.DomainModel.Entities
{
    public class Organization
    {
        public Organization()
        {
            Members = new List<User>();
            EmailDomains = new List<OrganizationEmailDomain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid RegistrationToken { get; set; }
        public bool IsEmailDomainRestricted { get; set; }

        public virtual ICollection<User> Members { get; set; }
        public virtual ICollection<OrganizationEmailDomain> EmailDomains { get; set; }
    }
}