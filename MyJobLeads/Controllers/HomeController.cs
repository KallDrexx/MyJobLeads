using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads.Controllers
{
    public partial class HomeController : MyJobLeadsBaseController
    {
        public HomeController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        public virtual ActionResult Index()
        {
            ViewBag.Message = "Welcome to My Job Leads!";

            if (User.Identity.IsAuthenticated)
            {
                var user = new UserByIdQuery(_unitOfWork).WithUserId(CurrentUserId).Execute();
                if (user == null)
                    return View(); // Only happens when Asp.net thinks a non-existant user is logged in

                if (user.LastVisitedJobSearchId == null)
                {
                    if (user.JobSearches.Count == 0)
                        return RedirectToAction(MVC.JobSearch.Add());
                    else
                        return RedirectToAction(MVC.JobSearch.Index());
                }

                return RedirectToAction(MVC.JobSearch.View(user.LastVisitedJobSearchId.Value));
            }

            
            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }
    }
}
