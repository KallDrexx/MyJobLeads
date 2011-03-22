using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(MVC.JobSearch.Index());

            ViewBag.Message = "Welcome to My Job Leads!";
            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }
    }
}
