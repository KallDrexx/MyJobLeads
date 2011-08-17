﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.ViewModels.Companies
{
    public class CompanyDisplayViewModel
    {
        public CompanyDisplayViewModel() { }

        public CompanyDisplayViewModel(Company company)
        {
            Id = company.Id;
            Name = company.Name;
            Phone = company.Phone;
            Notes = company.Notes;
            LeadStatus = company.LeadStatus;

            // Form company location string
            Location = string.Empty;

            bool citySpecified = !string.IsNullOrWhiteSpace(company.City);
            bool stateSpecified = !string.IsNullOrWhiteSpace(company.State);
            bool zipSpecified = !string.IsNullOrWhiteSpace(company.Zip);

            if (citySpecified && stateSpecified) { Location = string.Concat(company.City, ", ", company.State, " "); }
            else if (citySpecified) { Location = company.City + " "; }
            else if (stateSpecified) { Location = company.State + " "; }

            if (zipSpecified) { Location += company.Zip; }

            // Form the task list
            Tasks = company.Tasks.ToList()
                                 .Select(x => new CompanyTaskViewModel(x))
                                 .ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string LeadStatus { get; set; }

        public IList<CompanyTaskViewModel> Tasks { get; set; }

        public class CompanyTaskViewModel
        {
            public CompanyTaskViewModel(Task task)
            {
                Id = task.Id;
                Name = task.Name;
                Completed = task.CompletionDate != null;
                DueDate = task.TaskDate;
                Notes = task.Notes;

                AssociatedWith = task.Contact != null
                                ? task.Contact.Name
                                : task.Company.Name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public bool Completed { get; set; }
            public DateTime? DueDate { get; set; }
            public string AssociatedWith { get; set; }
            public string Notes { get; set; }
        }
    }
}