using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Extensions;

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
            Contacts = company.Contacts.Select(x => new CompanyContactViewModel(x)).ToList();
            Positions = company.Positions.Select(x => new CompanyPositionViewModel(x)).ToList();
            Location = company.LocationString();            

            // Form the task list
            OpenTasks = company.Tasks.Where(x => x.CompletionDate == null)
                                     .ToList()
                                     .Select(x => new CompanyTaskViewModel(x))
                                     .ToList();

            CompletedTasks = company.Tasks.Where(x => x.CompletionDate != null)
                                          .ToList()
                                          .Select(x => new CompanyTaskViewModel(x))
                                          .ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string LeadStatus { get; set; }
        public bool showPositions { get; set; }

        public IList<CompanyTaskViewModel> OpenTasks { get; set; }
        public IList<CompanyTaskViewModel> CompletedTasks { get; set; }
        public IList<CompanyContactViewModel> Contacts { get; set; }
        public IList<CompanyPositionViewModel> Positions { get; set; }

        public class CompanyTaskViewModel
        {
            public CompanyTaskViewModel(Task task)
            {
                Id = task.Id;
                Name = task.Name;
                CompletionDate = task.CompletionDate;
                DueDate = task.TaskDate;
                Notes = task.Notes;

                AssociatedWith = task.Contact != null
                                ? task.Contact.Name
                                : task.Company.Name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? CompletionDate { get; set; }
            public DateTime? DueDate { get; set; }
            public string AssociatedWith { get; set; }
            public string Notes { get; set; }
        }

        public class CompanyContactViewModel
        {
            public CompanyContactViewModel() { }

            public CompanyContactViewModel(Contact contact)
            {
                Id = contact.Id;
                Name = contact.Name;
                DirectPhone = contact.DirectPhone;
                MobilePhone = contact.MobilePhone;
                Notes = contact.Notes;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string DirectPhone { get; set; }
            public string MobilePhone { get; set; }
            public string Notes { get; set; }
        }

        public class CompanyPositionViewModel
        {
            public CompanyPositionViewModel() { }

            public CompanyPositionViewModel(Position position)
            {
                Id = position.Id;
                Title = position.Title;
                HasApplied = position.HasApplied;
                Notes = position.Notes;
            }

            public int Id { get; set; }
            public string Title { get; set; }
            public bool HasApplied { get; set; }
            public string Notes { get; set; }
        }
    }
}