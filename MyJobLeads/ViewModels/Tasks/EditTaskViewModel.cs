using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.ViewModels.Contacts;

namespace MyJobLeads.ViewModels.Tasks
{
    public class EditTaskViewModel
    {
        public EditTaskViewModel() 
        {
            AvailableCategoryList = new List<string>();
        }

        public EditTaskViewModel(Company company) 
        {
            Company = new CompanySummaryViewModel(company);
            AssociatedCompanyId = company.Id;
            AvailableCategoryList = new List<string>();
        }

        public EditTaskViewModel(Task task)
        {
            Id = task.Id;
            Name = task.Name;
            TaskDate = task.TaskDate;
            Completed = task.CompletionDate != null;
            AssociatedContactId = Convert.ToInt32(task.ContactId);
            Company = new CompanySummaryViewModel(task.Company);
            Contact = new ContactSummaryViewModel(task.Contact);
            Category = task.Category;
            Notes = task.Notes;

            AssociatedContactId = task.ContactId ?? 0;
            AvailableCategoryList = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public bool Completed { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Notes { get; set; }

        public CompanySummaryViewModel Company { get; set; }
        public ContactSummaryViewModel Contact { get; set; }

        public int AssociatedContactId { get; set; }
        public int AssociatedCompanyId { get; set; }

        public IList<SelectListItem> CompanyContactList { get; set; }
        public IList<string> AvailableCategoryList { get; set; }
    }
}