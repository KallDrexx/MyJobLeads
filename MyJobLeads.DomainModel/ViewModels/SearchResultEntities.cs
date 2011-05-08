using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.ViewModels
{
    public class SearchResultEntities
    {
        public SearchResultEntities()
        {
            Companies = new List<Company>();
            Contacts = new List<Contact>();
            Tasks = new List<Task>();
        }

        public IList<Company> Companies { get; set; }
        public IList<Contact> Contacts { get; set; }
        public IList<Task> Tasks { get; set; }
    }
}
