using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class CompanyHistory : EntityHistoryBase
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MetroArea { get; set; }
        public string Industry { get; set; }
        public string LeadStatus { get; set; }

        [StringLength(Int32.MaxValue)]
        public string Notes { get; set; }

        public virtual int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
