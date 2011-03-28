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
                    return View();

                if (user.LastVisitedJobSearchId == null)
                    return RedirectToAction(MVC.JobSearch.Index());

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
