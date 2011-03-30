using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MetroArea { get; set; }
        public string Industry { get; set; }
        public string Notes { get; set; }

        public virtual JobSearch JobSearch { get; set; }
        public virtual int? JobSearchID { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
