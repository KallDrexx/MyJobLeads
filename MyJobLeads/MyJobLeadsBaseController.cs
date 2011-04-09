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
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);

            if (MembershipService.GetUser() != null)
                CurrentUserId = (int)MembershipService.GetUser().ProviderUserKey;
        }

        public IMembershipService MembershipService { get; set; }

        public int CurrentUserId { get; set; }

        protected IUnitOfWork _unitOfWork;
    }
}