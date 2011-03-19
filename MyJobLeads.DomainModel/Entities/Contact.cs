using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DirectPhone { get; set; }
        public string MobilePhone { get; set; }
        public string Extension { get; set; }
        public string Email { get; set; }
        public string Assistant { get; set; }
        public string ReferredBy { get; set; }
        public string Notes { get; set; }

        public virtual Company Company { get; set; }
    }
}
