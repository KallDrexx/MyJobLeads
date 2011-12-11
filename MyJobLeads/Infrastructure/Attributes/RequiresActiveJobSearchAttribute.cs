using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.DomainModel.Entities.EF;
using System.Web.Security;
using System.Web.Routing;

namespace MyJobLeads.Infrastructure.Attributes
{
    public class RequiresActiveJobSearchAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Make sure the user has an active job search
            if (Membership.GetUser() != null)
            {
                var container = new WindsorContainer();
                container.Install(FromAssembly.Containing<RequiresActiveJobSearchAttribute>());
                var context = container.Resolve<MyJobLeadsDbContext>();
                int id = (int)Membership.GetUser().ProviderUserKey;
                var user = context.Users.SingleOrDefault(x => x.Id == id);

                if (user != null && user.LastVisitedJobSearchId != null)
                    return;
            }

            // If no job search is active, redirect home
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{"controller", "Home"}, {"action", "Index"}, {"area", "" }});
        }
    }
}