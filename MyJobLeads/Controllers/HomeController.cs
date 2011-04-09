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
        public HomeController(UserByIdQuery userByIdQuery)
        {
            _userByIdQuery = userByIdQuery;
        }

        private UserByIdQuery _userByIdQuery;

        public virtual ActionResult Index()
        {
            if (CurrentUserId != 0)
            {
                var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();

                if (user.LastVisitedJobSearchId == null)
                {
                    if (user.JobSearches.Count == 0)
                        return RedirectToAction(MVC.JobSearch.Add());
                    else
                        return RedirectToAction(MVC.JobSearch.Index());
                }

                return RedirectToAction(MVC.JobSearch.Details(user.LastVisitedJobSearchId.Value));
            }

            
            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }
    }
}
