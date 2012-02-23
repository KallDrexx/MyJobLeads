using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using System.Web.Security;
using Castle.Windsor.Installer;
using MyJobLeads.DomainModel.Entities.EF;
using System.Web.Routing;

namespace MyJobLeads.Infrastructure.Attributes
{
    public class RequiresOrganizationAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Only allow access if the user is a site administrator
            if (Membership.GetUser() != null)
            {
                var container = new WindsorContainer();
                container.Install(FromAssembly.Containing<RequiresOrganizationAdminAttribute>());

                var context = container.Resolve<MyJobLeadsDbContext>();
                int userId = (int)Membership.GetUser().ProviderUserKey;

                if (context.Users.Where(x => x.Id == userId && x.IsOrganizationAdmin).Count() > 0)
                    return;
            }

            // User wasn't logged in or not an organization administrator, so redirect to the homepage
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
            { 
                { "controller", "Home" }, 
                { "action", "Index" },
                { "area", "" }
            });
        }
    }
}