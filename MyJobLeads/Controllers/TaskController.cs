using System;
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
using MyJobLeads.ViewModels.Contacts;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.DomainModel.ProcessParams.JobSearches;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ViewModels.JobSearches;
using MyJobLeads.DomainModel.Queries.JobSearches;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    [RequiresActiveJobSearch]
    public partial class TaskController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;
        protected IProcess<JobSearchMilestonePropogationParams, JobSearchMilestoneChangedResultViewModel> _jobsearchPropogationProcess;

        public TaskController(IServiceFactory factory,
                                IProcess<JobSearchMilestonePropogationParams, JobSearchMilestoneChangedResultViewModel> jobsearchPropogationProcess)
        {
            _serviceFactory = factory;
            _jobsearchPropogationProcess = jobsearchPropogationProcess;
        }

        #region Actions 

        public virtual ActionResult Index()
        {
            // NOTE: The user.JobSearch property must NOT be called until after the propogation process,
            //   otherwise the job search entity won't have the propogated milestone
            JobSearch search;

            // Make sure the user has a created job search
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            if (user.LastVisitedJobSearchId == null)
                return RedirectToAction(MVC.JobSearch.Add());

            // Update the job search's current milestone if required
            var result = _jobsearchPropogationProcess.Execute(new JobSearchMilestonePropogationParams { JobSearchId = (int)user.LastVisitedJobSearchId });

            // If the job search's current milestone was updated, we need to reload the job search
            if (result.JobSearchMilestoneChanged)
                search = _serviceFactory.GetService<JobSearchByIdQuery>().WithJobSearchId((int)user.LastVisitedJobSearchId).Execute();
            else
                search = user.LastVisitedJobSearch;

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
            var company = companyByIdQuery.WithCompanyId(companyId).RequestedByUserId(CurrentUserId).Execute();
            if (company == null)
            {
                ViewBag.EntityType = "Company";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            // Form the view model
            var model = new EditTaskViewModel(company);
            model.AvailableCategoryList = _serviceFactory.GetService<CategoriesAvailableForTasksQuery>().Execute();

            if (contactId != 0)
            {
                model.Contact = new ContactSummaryViewModel(
                                        _serviceFactory.GetService<ContactByIdQuery>()
                                                       .WithContactId(contactId)
                                                       .RequestedByUserId(CurrentUserId)
                                                       .Execute());
                model.AssociatedContactId = contactId;
            }

            // Create contact list
            CreateCompanyContactList(contactId, company, model);

            return View(MVC.Task.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            // Retrieve the task
            var taskByIdQuery = _serviceFactory.GetService<TaskByIdQuery>();
            var task = taskByIdQuery.WithTaskId(id).RequestedByUserId(CurrentUserId).Execute();
            if (task == null)
            {
                ViewBag.EntityType = "Task";
                return View(MVC.Shared.Views.EntityNotFound);
            }

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
                var contact = _serviceFactory.GetService<ContactByIdQuery>().WithContactId(selectedContactId).RequestedByUserId(CurrentUserId).Execute();
                var company = _serviceFactory.GetService<CompanyByIdQuery>().WithCompanyId(model.AssociatedCompanyId).RequestedByUserId(CurrentUserId).Execute();

                if (contact == null)
                {
                    ViewBag.EntityType = "Contact";
                    return View(MVC.Shared.Views.EntityNotFound);
                }

                if (company == null)
                {
                    ViewBag.EntityType = "Company";
                    return View(MVC.Shared.Views.EntityNotFound);
                }

                model.Contact = new ContactSummaryViewModel(contact);
                model.Company = new CompanySummaryViewModel(company);

                model.AvailableCategoryList = _serviceFactory.GetService<CategoriesAvailableForTasksQuery>().Execute();
                CreateCompanyContactList(Convert.ToInt32(model.AssociatedContactId), company, model);

                // Re-show form
                return View(model);
            }
        }

        public virtual ActionResult Details(int id)
        {
            var task = _serviceFactory.GetService<TaskByIdQuery>().WithTaskId(id).RequestedByUserId(CurrentUserId).Execute();
            if (task == null)
            {
                ViewBag.EntityType = "Task";
                return View(MVC.Shared.Views.EntityNotFound);
            }

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
                                                                                            .RequestedByUserId(CurrentUserId)
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
