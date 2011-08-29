using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.ViewModels.Accounts;
using System.Web.Routing;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities.EF;

namespace MyJobLeads
{
    public class MyJobLeadsBaseController : Controller
    {
        public MyJobLeadsBaseController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Membership.GetUser() != null)
                CurrentUserId = (int)Membership.GetUser().ProviderUserKey;
        }

        public int CurrentUserId { get; set; }

        protected IUnitOfWork _unitOfWork;
        protected MyJobLeadsDbContext _context;
        protected IServiceFactory _serviceFactory;
    }
}