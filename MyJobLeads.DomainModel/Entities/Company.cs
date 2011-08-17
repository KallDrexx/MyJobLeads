using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.History;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.Entities
{
    public class Company
    {
        public Company()
        {
            Contacts = new List<Contact>();
            Tasks = new List<Task>();
            History = new List<CompanyHistory>();
            Positions = new List<Position>();
        }

        public int Id { get; set; }
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

        public virtual JobSearch JobSearch { get; set; }
        public virtual int? JobSearchID { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<CompanyHistory> History { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
