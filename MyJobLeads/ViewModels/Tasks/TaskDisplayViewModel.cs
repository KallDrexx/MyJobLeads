using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.ViewModels.Contacts;

namespace MyJobLeads.ViewModels.Tasks
{
    public class TaskDisplayViewModel
    {
        public TaskDisplayViewModel() { }

        public TaskDisplayViewModel(Task task)
        {
            Id = task.Id;
            Name = task.Name;
            TaskDueDate = task.TaskDate;
            TaskDueDateString = task.TaskDate != null ? task.TaskDate.Value.ToLongDateString() : string.Empty;
            CompletionDate = task.CompletionDate;
            Category = task.Category;
            Notes = task.Notes;
            AssociatedWith = task.ContactId == null ? task.Company.Name : task.Contact.Name;

            Company = new CompanySummaryViewModel(task.Company);
            Contact = new ContactSummaryViewModel(task.Contact);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public string TaskDueDateString { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Notes { get; set; }
        public string AssociatedWith { get; set; }
        public string Category { get; set; }

        public CompanySummaryViewModel Company { get; set; }
        public ContactSummaryViewModel Contact { get; set; }
    }
}