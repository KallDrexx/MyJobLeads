﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.ViewModels.Tasks;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class TaskController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;
        protected IServiceFactory _serviceFactory;

        public TaskController(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        #region Actions 

        public virtual ActionResult Add(int companyId, int contactId = 0)
        {
            var companyByIdQuery = _serviceFactory.GetService<CompanyByIdQuery>();

            // Retrieve the specified company value
            var company = companyByIdQuery.WithCompanyId(companyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), companyId);

            // Form the view model
            var model = new EditTaskViewModel(company);

            // Create contact list
            CreateCompanyContactList(contactId, company, model);

            return View(MVC.Task.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var taskByIdQuery = _serviceFactory.GetService<TaskByIdQuery>();

            var task = taskByIdQuery.WithTaskId(id).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), id);

            // Form the view model
            var model = new EditTaskViewModel(task);

            // Create contact list
            CreateCompanyContactList(Convert.ToInt32(task.ContactId), task.Company, model);

            return View(MVC.Task.Views.Edit, model);
        }
        
        [HttpPost]
        public virtual ActionResult Edit(EditTaskViewModel model)
        {
            Task task;
            int selectedContactId = model.AssociatedContactId == -1 ? 0 : model.AssociatedContactId;

            // Determine if this is a new task or not
            if (model.Id == 0)
            {
                task = _serviceFactory.GetService<CreateTaskCommand>().WithCompanyId(model.AssociatedCompanyId)
                                                         .SetName(model.Name)
                                                         .SetTaskDate(model.TaskDate)
                                                         .SetCategory(model.Category)
                                                         .SetSubCategory(model.SubCategory)
                                                         .WithContactId(selectedContactId)
                                                         .RequestedByUserId(CurrentUserId)
                                                         .Execute();
            }
            else
            {
                // Existing task
                task = _serviceFactory.GetService<EditTaskCommand>().WithTaskId(model.Id)
                                                       .SetName(model.Name)
                                                       .SetTaskDate(model.TaskDate)
                                                       .SetContactId(selectedContactId)
                                                       .SetCompleted(model.Completed)
                                                       .SetCategory(model.Category)
                                                       .SetSubCategory(model.SubCategory)
                                                       .RequestedByUserId(CurrentUserId)
                                                       .Execute();
            }

            return RedirectToAction(MVC.Task.Details(task.Id));
        }

        public virtual ActionResult Details(int id)
        {
            var task = _serviceFactory.GetService<TaskByIdQuery>().WithTaskId(id).Execute();
            return View(task);
        }

        #endregion

        #region Utility Methods

        protected void CreateCompanyContactList(int selectedContactId, Company company, EditTaskViewModel model)
        {
            model.CompanyContactList = _serviceFactory.GetService<ContactsByCompanyIdQuery>().WithCompanyId(company.Id)
                                                                                            .Execute()
                                                                                            .Select(x => new SelectListItem
                                                                                            {
                                                                                                Text = x.Name,
                                                                                                Value = x.Id.ToString(),
                                                                                                Selected = x.Id == selectedContactId
                                                                                            })
                                                                                            .ToList();
            model.CompanyContactList.Insert(0, new SelectListItem { Text = "---------------------------", Value = "-1", Selected = false });
            model.CompanyContactList.Insert(0, new SelectListItem { Text = "Company: " + company.Name, Value = "0", Selected = selectedContactId == 0 });
        }

        #endregion
    }
}
