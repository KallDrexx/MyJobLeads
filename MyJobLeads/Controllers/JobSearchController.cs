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

        public JobSearchController(JobSearchesByUserIdQuery jobSearchesByIdQuery,
                                    JobSearchByIdQuery jobSearchByIdQuery,
                                    CreateJobSearchForUserCommand createJobSearchCommand,
                                    EditJobSearchCommand editJobSearchCommand,
                                    OpenTasksByJobSearchQuery openTasksByJobSearchQuery,
                                    EditUserCommand editUserCommand,
                                    EntitySearchQuery entitySearchQuery)
        {
            _jobSearchByIdQuery = jobSearchByIdQuery;
            _jobSearchesByUserIdQuery = jobSearchesByIdQuery;
            _createJobSearchCommand = createJobSearchCommand;
            _editJobSearchCommand = editJobSearchCommand;
            _openTasksByJobSearchQuery = openTasksByJobSearchQuery;
            _editUserCommand = editUserCommand;
            _entitySearchQuery = entitySearchQuery;
        }

        #endregion

        public virtual ActionResult Index()
        {
            var searches = _jobSearchesByUserIdQuery.WithUserId(CurrentUserId).Execute();
            return View(searches);
        }

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
            // Determine if we are editing or adding a jobsearch
            if (jobSearch.Id == 0)
            {
                jobSearch = _createJobSearchCommand.ForUserId(CurrentUserId)
                                                    .WithName(jobSearch.Name)
                                                    .WithDescription(jobSearch.Description)
                                                    .Execute();
                return RedirectToAction(MVC.JobSearch.Details(jobSearch.Id));
            }
            else
            {
                _editJobSearchCommand.WithJobSearchId(jobSearch.Id)
                                    .SetName(jobSearch.Name)
                                    .SetDescription(jobSearch.Description)
                                    .CalledByUserId(CurrentUserId)
                                    .Execute();
                return RedirectToAction(MVC.JobSearch.Details(jobSearch.Id));
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

        public virtual ActionResult Search(int id, string query)
        {
            var results = _entitySearchQuery.WithJobSearchId(id).WithSearchQuery(query).Execute();
            return View(new PerformedSearchViewModel { SearchQuery = query, Results = results, JobSearchId = id });
        }
    }
}
