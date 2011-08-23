using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.ViewModels.Companies;

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
            Completed = task.CompletionDate != null;
            Category = task.Category;
            Notes = task.Notes;
            AssociatedWith = task.ContactId == null ? task.Company.Name : task.Contact.Name;

            Company = new CompanyDisplayViewModel(task.Company);
            Contact = task.Contact;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public string TaskDueDateString { get; set; }
        public bool Completed { get; set; }
        public string Notes { get; set; }
        public string AssociatedWith { get; set; }
        public string Category { get; set; }

        public CompanyDisplayViewModel Company { get; set; }
        public Contact Contact { get; set; }
    }
}