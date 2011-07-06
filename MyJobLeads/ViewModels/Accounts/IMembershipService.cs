using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MyJobLeads.ViewModels.Accounts
{
    public struct CreateUserMembershipParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? OrganizationRegistrationToken { get; set; }
        public string FullName { get; set; }
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(CreateUserMembershipParams userParams);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipUser GetUser();
    }
}