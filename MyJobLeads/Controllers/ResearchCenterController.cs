using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Controllers
{
    public partial class ResearchCenterController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
