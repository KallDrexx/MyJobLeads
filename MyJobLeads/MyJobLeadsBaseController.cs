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
        }

        public IMembershipService MembershipService { get; set; }
        public int CurrentUserId 
        {
            get
            {
                var user = MembershipService.GetUser();
                if (user != null)
                    return (int)user.ProviderUserKey;
                else
                    return 0;
            }
        }
        

        protected IUnitOfWork _unitOfWork;
    }
}