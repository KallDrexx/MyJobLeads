using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.ViewModels.Companies
{
    public class EditCompanyViewModel
    {
        public EditCompanyViewModel() 
        { 
            // Set the Lead Status to default to Prospective Employer
            LeadStatus = "Prospective Employer";
        }

        public EditCompanyViewModel(Company company)
        {
            Id = company.Id;
            Name = company.Name;
            Phone = company.Phone;
            City = company.City;
            State = company.State;
            Zip = company.Zip;
            MetroArea = company.MetroArea;
            Industry = company.Industry;
            LeadStatus = company.LeadStatus;
            Notes = company.Notes;

            JobSearchId = Convert.ToInt32(company.JobSearchID);
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
        public string Notes { get; set; }

        public int JobSearchId { get; set; }

        public IList<string> AvailableLeadStatuses { get; set; }
    }
}