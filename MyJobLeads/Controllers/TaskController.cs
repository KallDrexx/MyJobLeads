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
using FluentValidation;
using MyJobLeads.DomainModel.Queries.Users;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class TaskController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;

        public TaskController(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        #region Actions 

        public virtual ActionResult Index()
        {
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            var model = new TaskDashboardViewModel(user);

            if (model.TodaysTasks.Count > 0)
                return View(MVC.Task.Views.Today, model);
            
            if (model.OverdueTasks.Count > 0)
                return View(MVC.Task.Views.Overdue, model);

            if (model.FutureTasks.Count > 0)
                return View(MVC.Task.Views.Future, model);

            // If no uncompleted tasks exist, go to the today view
            return View(MVC.Task.Views.Today, model);
        }

        public virtual ActionResult Add(int companyId, int contactId = 0)
        {
            var companyByIdQuery = _serviceFactory.GetService<CompanyByIdQuery>();

            // Retrieve the specified company value
            var company = companyByIdQuery.WithCompanyId(companyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), companyId);

            // Form the view model
            var model = new EditTaskViewModel(company);
            model.AvailableCategoryList = _serviceFactory.GetService<CategoriesAvailableForTasksQuery>().Execute();

            // Create contact list
            CreateCompanyContactList(contactId, company, model);

            return View(MVC.Task.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            // Retrieve the task
            var taskByIdQuery = _serviceFactory.GetService<TaskByIdQuery>();
            var task = taskByIdQuery.WithTaskId(id).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), id);

            // Get the list of available task categories
            var categories = _serviceFactory.GetService<CategoriesAvailableForTasksQuery>().Execute();

            // Form the view model
            var model = new EditTaskViewModel(task);
            model.AvailableCategoryList = categories;

            // Create contact list
            CreateCompanyContactList(Convert.ToInt32(task.ContactId), task.Company, model);

            return View(MVC.Task.Views.Edit, model);
        }
        
        [HttpPost]
        public virtual ActionResult Edit(EditTaskViewModel model)
        {
            Task task;
            int selectedContactId = model.AssociatedContactId == -1 ? 0 : model.AssociatedContactId;

            try
            {
                // Determine if this is a new task or not
                if (model.Id == 0)
                {
                    task = _serviceFactory.GetService<CreateTaskCommand>().Execute(new CreateTaskCommandParams
                    {
                        CompanyId = model.AssociatedCompanyId,
                        Name = model.Name,
                        Category = model.Category,
                        TaskDate = model.TaskDate,
                        ContactId = selectedContactId,
                        RequestedUserId = CurrentUserId,
                        Notes = model.Notes
                    });
                }
                else
                {
                    // Existing task
                    task = _serviceFactory.GetService<EditTaskCommand>().Execute(new EditTaskCommandParams
                    {
                        TaskId = model.Id,
                        TaskDate = model.TaskDate,
                        Name = model.Name,
                        ContactId = selectedContactId,
                        Completed = model.Completed,
                        Category = model.Category,
                        RequestingUserId = CurrentUserId,
                        Notes = model.Notes
                    });
                }

                return RedirectToAction(MVC.Task.Details(task.Id));
            }

            catch (ValidationException ex)
            {
                // Add all the errors to the model state
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                // Re-retrieve the contact and company entities from the database
                model.Contact = _serviceFactory.GetService<ContactByIdQuery>().WithContactId(selectedContactId).Execute();
                model.Company = _serviceFactory.GetService<CompanyByIdQuery>().WithCompanyId(model.AssociatedCompanyId).Execute();
                model.AvailableCategoryList = _serviceFactory.GetService<CategoriesAvailableForTasksQuery>().Execute();
                CreateCompanyContactList(Convert.ToInt32(model.AssociatedContactId), model.Company, model);

                // Re-show form
                return View(model);
            }
        }

        public virtual ActionResult Details(int id)
        {
            var task = _serviceFactory.GetService<TaskByIdQuery>().WithTaskId(id).Execute();
            var model = new TaskDisplayViewModel(task);
            return View(model);
        }

        public virtual ActionResult Overdue()
        {
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            var model = new TaskDashboardViewModel(user);

            return View(model);
        }

        public virtual ActionResult Today()
        {
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            var model = new TaskDashboardViewModel(user);

            return View(model);
        }

        public virtual ActionResult Future()
        {
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            var model = new TaskDashboardViewModel(user);

            return View(model);
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
