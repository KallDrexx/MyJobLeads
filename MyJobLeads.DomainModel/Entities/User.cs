using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MyJobLeads.DomainModel.Entities.Ordering;

namespace MyJobLeads.DomainModel.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool IsOrganizationAdmin { get; set; }
        public bool IsSiteAdmin { get; set; }
        public Guid? FillPerfectKey { get; set; }

        public int? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual ICollection<JobSearch> JobSearches { get; set; }
        public virtual ICollection<Order> CreatedOrders { get; set; }
        public virtual ICollection<Order> OwnedOrders { get; set; }
        
        public int? LastVisitedJobSearchId { get; set; }
        public virtual JobSearch LastVisitedJobSearch { get; set; }

        public int? LinkedInOAuthDataId { get; set; }
        public virtual OAuthData LinkedInOAuthData { get; set; }

        public virtual JigsawAccountDetails JigsawAccountDetails { get; set; }

    }
}
