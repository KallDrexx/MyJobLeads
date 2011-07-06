using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MyJobLeads.Infrastructure;

namespace MyJobLeads.ViewModels.Accounts
{
    public class AccountMembershipService : IMembershipService
    {
        private readonly MyJobLeadsMembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            // Only support the MyJobLeadsMembershipProvider
            var tempProvider = provider ?? Membership.Provider;
            _provider = tempProvider as MyJobLeadsMembershipProvider;
            
            if (_provider == null)
                throw new InvalidOperationException(
                    string.Format("Membership provider of type {0} was provided but only {1} is supported",
                        tempProvider.GetType(), typeof(MyJobLeadsMembershipProvider)));
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(CreateUserMembershipParams userParams)
        {
            if (String.IsNullOrEmpty(userParams.Password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(userParams.Email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userParams, out status);
            
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public MembershipUser GetUser()
        {
            return Membership.GetUser();
        }
    }
}