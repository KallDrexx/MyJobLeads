using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Configuration
{
    public class OrganizationEmailDomain
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public bool IsActive { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual int? OrganizationId { get; set; }
    }
}
