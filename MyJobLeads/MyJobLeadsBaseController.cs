using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.Models.Accounts;
using System.Web.Routing;

namespace MyJobLeads
{
    public class MyJobLeadsBaseController : Controller
    {
        public MyJobLeadsBaseController()
        {
            CurrentUserId = 0;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);

            // If the user is logged in, retrieve their login id
            var user = MembershipService.GetUser();
            if (user != null)
                CurrentUserId = (int)user.ProviderUserKey;
        }

        public int CurrentUserId { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected IUnitOfWork _unitOfWork;
    }
}