using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MyJobLeads.ViewModels.Accounts
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string email, string password, Guid? orgRegistrationToken);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipUser GetUser();
    }
}