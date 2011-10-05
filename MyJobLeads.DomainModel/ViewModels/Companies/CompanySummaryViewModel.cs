using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Extensions;

namespace MyJobLeads.ViewModels.Companies
{
    public class CompanySummaryViewModel
    {
        public CompanySummaryViewModel() { }

        public CompanySummaryViewModel(Company company)
        {
            if (company == null)
                throw new ArgumentNullException("company");

            Id = company.Id;
            Name = company.Name;
            Phone = company.Phone;
            Notes = company.Notes;
            Location = company.LocationString();
            LeadStatus = company.LeadStatus;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string LeadStatus { get; set; }
        public string Notes { get; set; }
    }
}