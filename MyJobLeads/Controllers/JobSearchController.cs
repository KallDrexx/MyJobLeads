using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.JobSearches;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.ViewModels.JobSearches;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Queries.Search;
using MyJobLeads.ViewModels;
using MyJobLeads.DomainModel.Providers;
using FluentValidation;
using MyJobLeads.DomainModel.ViewModels.Exports;
using MyJobLeads.DomainModel.ProcessParams.JobSearches;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class JobSearchController : MyJobLeadsBaseController
    {
        #region Constructor and Command/Query Members

        protected JobSearchesByUserIdQuery _jobSearchesByUserIdQuery;
        protected JobSearchByIdQuery _jobSearchByIdQuery;
        protected CreateJobSearchForUserCommand _createJobSearchCommand;
        protected EditJobSearchCommand _editJobSearchCommand;
        protected OpenTasksByJobSearchQuery _openTasksByJobSearchQuery;
        protected EditUserCommand _editUserCommand;
        protected EntitySearchQuery _entitySearchQuery;
        protected UserByIdQuery _userByIdQuery;
        protected StartNextJobSearchMilestoneCommand _startNextMilestoneCmd;
        protected IProcess<ByJobSearchParams, JobsearchExportViewModel> _exportProcess;

        public JobSearchController(JobSearchesByUserIdQuery jobSearchesByIdQuery,
                                    JobSearchByIdQuery jobSearchByIdQuery,
                                    CreateJobSearchForUserCommand createJobSearchCommand,
                                    EditJobSearchCommand editJobSearchCommand,
                                    OpenTasksByJobSearchQuery openTasksByJobSearchQuery,
                                    EditUserCommand editUserCommand,
                                    EntitySearchQuery entitySearchQuery,
                                    UserByIdQuery userByIdQuery,
                                    StartNextJobSearchMilestoneCommand startNextMilestoneCmd,
                                    IProcess<ByJobSearchParams, JobsearchExportViewModel> exportProcess,
                                    IServiceFactory serviceFactory)
        {
            _jobSearchByIdQuery = jobSearchByIdQuery;
            _jobSearchesByUserIdQuery = jobSearchesByIdQuery;
            _createJobSearchCommand = createJobSearchCommand;
            _editJobSearchCommand = editJobSearchCommand;
            _openTasksByJobSearchQuery = openTasksByJobSearchQuery;
            _editUserCommand = editUserCommand;
            _entitySearchQuery = entitySearchQuery;
            _serviceFactory = serviceFactory;
            _userByIdQuery = userByIdQuery;
            _startNextMilestoneCmd = startNextMilestoneCmd;
            _exportProcess = exportProcess;
        }

        #endregion

        public virtual ActionResult Add()
        {
            return View(MVC.JobSearch.Views.Edit, new JobSearch());
        }

        public virtual ActionResult Edit(int id)
        {
            var jobSearch = _jobSearchByIdQuery.WithJobSearchId(id).Execute();
            return View(jobSearch);
        }

        [HttpPost]
        public virtual ActionResult Edit(JobSearch jobSearch)
        {
            try
            {
                // Determine if we are editing or adding a jobsearch
                if (jobSearch.Id == 0)
                {
                    jobSearch = _createJobSearchCommand.ForUserId(CurrentUserId)
                                                        .WithName(jobSearch.Name)
                                                        .WithDescription(jobSearch.Description)
                                                        .Execute();
                }
                else
                {
                    _editJobSearchCommand.WithJobSearchId(jobSearch.Id)
                                        .SetName(jobSearch.Name)
                                        .SetDescription(jobSearch.Description)
                                        .CalledByUserId(CurrentUserId)
                                        .Execute();
                }

                // Set the current user's last visited job search to this one
                _editUserCommand.WithUserId(CurrentUserId).SetLastVisitedJobSearchId(jobSearch.Id).Execute();
                return RedirectToAction(MVC.Task.Index());
            }
            
            // Show validation errors to the user and allow them to fix them
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return View(jobSearch);
            }
        }

        public virtual ActionResult Details(int id)
        {
            // Retrieve the specified job search information
            var search = _jobSearchByIdQuery.WithJobSearchId(id).Execute();
            var openTasks = _openTasksByJobSearchQuery.WithJobSearch(id).Execute();

            // Set this as the user's last visited job search
            _editUserCommand.WithUserId(CurrentUserId).SetLastVisitedJobSearchId(id).Execute();

            return View(new JobSearchDetailsViewModel { JobSearch = search, OpenTasks = openTasks });
        }

        public virtual ActionResult Search(string query)
        {
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();
            var results = _entitySearchQuery.WithJobSearchId((int)user.LastVisitedJobSearchId).WithSearchQuery(query).Execute();

            TempData["LastSearchQuery"] = query;
            return View(new PerformedSearchViewModel { SearchQuery = query, Results = results, JobSearchId = (int)user.LastVisitedJobSearchId });
        }

        public virtual ActionResult StartNextMilestone()
        {
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();
            _startNextMilestoneCmd.Execute(new StartNextJobSearchMilestoneCommandParams { JobSearchId = Convert.ToInt32(user.LastVisitedJobSearchId) });
            return RedirectToAction(MVC.Home.Index());
        }

        public virtual ActionResult Export()
        {
            JobsearchExportViewModel export;
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();

            try { export = _exportProcess.Execute(new ByJobSearchParams { JobSearchId = Convert.ToInt32(user.LastVisitedJobSearchId) }); }
            catch (MJLEntityNotFoundException)
            {
                return RedirectToAction(MVC.Home.Index());
            }

            return File(export.ExportFileContents, export.Mimetype, export.FileName);
        }
    }
}
