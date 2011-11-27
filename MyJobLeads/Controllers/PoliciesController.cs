using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Controllers
{
    public partial class PoliciesController : Controller
    {
        public virtual ActionResult Privacy()
        {
            return View();
        }

        public virtual ActionResult Security()
        {
            return View();
        }
    }
}
