﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

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
            Company = company;
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
            Company = task.Company;
            Contact = task.Contact;
            Category = task.Category;
            SubCategory = task.SubCategory;

            AssociatedContactId = task.ContactId ?? 0;
            AvailableCategoryList = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public bool Completed { get; set; }
        public Company Company { get; set; }
        public Contact Contact { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public int AssociatedContactId { get; set; }
        public int AssociatedCompanyId { get; set; }

        public IList<SelectListItem> CompanyContactList { get; set; }
        public IList<string> AvailableCategoryList { get; set; }
    }
}