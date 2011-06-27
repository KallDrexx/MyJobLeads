﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? LastVisitedJobSearchId { get; set; }

        public virtual int? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual ICollection<JobSearch> JobSearches { get; set; }
    }
}
