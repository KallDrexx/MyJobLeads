using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class ContactHistory : EntityHistoryBase
    {
        public string Name { get; set; }
        public string DirectPhone { get; set; }
        public string MobilePhone { get; set; }
        public string Extension { get; set; }
        public string Email { get; set; }
        public string Assistant { get; set; }
        public string ReferredBy { get; set; }

        [StringLength(Int32.MaxValue)]
        public string Notes { get; set; }

        public virtual int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
