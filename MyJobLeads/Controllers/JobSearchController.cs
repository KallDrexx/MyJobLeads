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

namespace MyJobLeads.Controllers
{
    public partial class JobSearchController : MyJobLeadsBaseController
    {
        public JobSearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual ActionResult Index()
        {
            var searches = new JobSearchesByUserIdQuery(_unitOfWork).WithUserId(CurrentUserId).Execute();
            return View(searches);
        }

        public virtual ActionResult Add()
        {
            return View(MVC.JobSearch.Views.Edit, new JobSearch());
        }

        public virtual ActionResult Edit(int id)
        {
            var jobSearch = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(id).Execute();
            return View(jobSearch);
        }

        [HttpPost]
        public virtual ActionResult Edit(JobSearch jobSearch)
        {
            // Determine if we are editing or adding a jobsearch
            if (jobSearch.Id == 0)
            {
                jobSearch = new CreateJobSearchForUserCommand(_unitOfWork).ForUserId(CurrentUserId)
                                                                      .WithName(jobSearch.Name)
                                                                      .WithDescription(jobSearch.Description)
                                                                      .Execute();
                return RedirectToAction(MVC.JobSearch.View(jobSearch.Id));
            }
            else
            {
                new EditJobSearchCommand(_unitOfWork).WithJobSearchId(jobSearch.Id)
                                                     .SetName(jobSearch.Name)
                                                     .SetDescription(jobSearch.Description)
                                                     .Execute();
                return RedirectToAction(MVC.JobSearch.View(jobSearch.Id));
            }
        }

        public virtual ActionResult View(int id)
        {
            // Retrieve the specified job search
            var search = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(id).Execute();
            ViewBag.OpenTasks = new OpenTasksByJobSearchQuery(_unitOfWork).WithJobSearch(id).Execute();

            // Set this as the user's last visited job search
            new EditUserCommand(_unitOfWork).WithUserId(CurrentUserId).SetLastVisitedJobSearchId(id).Execute();

            return View(search);
        }

        public virtual ActionResult Search(string searchTerm)
        {
            return View();
        }
    }
}
