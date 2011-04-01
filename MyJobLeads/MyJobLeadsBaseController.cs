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

        private int? _currentUserId;
        public int CurrentUserId 
        {
            get
            {
                if (_currentUserId == null)
                {
                    var user = MembershipService.GetUser();
                    if (user != null)
                        _currentUserId = (int)MembershipService.GetUser().ProviderUserKey;
                    else
                        _currentUserId = 0;
                }

                return _currentUserId.Value;
            }
        }

        protected IUnitOfWork _unitOfWork;
    }
}