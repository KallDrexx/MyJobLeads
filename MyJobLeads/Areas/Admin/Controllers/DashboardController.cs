using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.Admin.Controllers
{
    [RequiresSiteAdmin]
    public partial class DashboardController : MyJobLeadsBaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
