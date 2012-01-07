using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Queries.Users;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.DomainModel.Data;
using System.Web.Security;

namespace MyJobLeads.Infrastructure.Attributes
{
    public class MJLAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) { throw new ArgumentNullException("httpContext"); }

            // Make sure Forms authentication shows the user as authenticated
            if (httpContext.User.Identity.IsAuthenticated == false) return false;
            if (Membership.GetUser() == null) return false;

            // Retrieve a unit of work from Windsor, and determine if the user actually exists in the database
            var container = new WindsorContainer();
            container.Install(FromAssembly.Containing<MJLAuthorizeAttribute>());
            var unitOfWork = container.Resolve<IUnitOfWork>();
            var user = new UserByIdQuery(unitOfWork).WithUserId((int)Membership.GetUser().ProviderUserKey).Execute();

            return user != null;
        }
    }
}