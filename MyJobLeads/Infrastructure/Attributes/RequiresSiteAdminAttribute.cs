using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using System.Web.Routing;

namespace MyJobLeads.Infrastructure.Attributes
{
    public class RequiresSiteAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Only allow access if the user is a site administrator
            if (Membership.GetUser() != null)
            {
                var container = new WindsorContainer();
                container.Install(FromAssembly.Containing<RequiresSiteAdminAttribute>());

                var process = container.Resolve<IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel>>();
                int userId = (int)Membership.GetUser().ProviderUserKey;
                var result = process.Execute(new SiteAdminAuthorizationParams { UserId = userId });

                if (result.UserAuthorized)
                    return;
            }

            // User wasn't logged in or not a site administrator, so redirect to the homepage
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
            { 
                { "controller", "Home" }, 
                { "action", "Index" },
                { "area", "" }
            });
        }
    }
}