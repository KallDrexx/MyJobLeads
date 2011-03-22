using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads
{
    public class MyJobLeadsBaseController : Controller
    {
        public MyJobLeadsBaseController()
        {
            // Set the CurrentUserId for the currently logged in user, if one is logged in
            if (Membership.GetUser() == null)
                CurrentUserId = 0;
            else
                CurrentUserId = (int)Membership.GetUser().ProviderUserKey;
        }

        /// <summary>
        /// Retrieves the currently logged in user id value
        /// </summary>
        public int CurrentUserId { get; set; }

        protected IUnitOfWork _unitOfWork;
    }
}