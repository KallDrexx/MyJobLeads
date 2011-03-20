using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Membership
{
    /// <summary>
    /// Class which converts a MyJobLeads user into an asp.net membership user
    /// </summary>
    public class MyJobLeadsMembershipUser : MembershipUser
    {
        public MyJobLeadsMembershipUser(User mjlUser)
            : base("MyJobLeadsMembershipUser"
            , mjlUser.Email
            , mjlUser.Id
            , mjlUser.Email
            , string.Empty // Not implemented
            , string.Empty // Not Implemented
            , true  // Not Implemented
            , false  // Not Implemented
            , DateTime.MinValue  // Not Implemented
            , DateTime.MinValue  // Not Implemented
            , DateTime.MinValue  // Not Implemented
            , DateTime.MinValue  // Not Implemented
            , DateTime.MinValue  // Not Implemented
            ) { }   
    }
}