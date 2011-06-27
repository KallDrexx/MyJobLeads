using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.ViewModels.Accounts;
using MyJobLeads.DomainModel.Entities;
using System.Web.Security;
using Moq;

namespace MyJobLeads.Tests.Mocks
{
    public class MockMembershipService : IMembershipService
    {
        protected User _user;

        public MockMembershipService(User user)
        {
            _user = user;
        }

        public int MinPasswordLength
        {
            get { return 10; }
        }

        public bool ValidateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public MembershipCreateStatus CreateUser(string email, string password, Guid? orgRegistrationToken)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public MembershipUser GetUser()
        {
            if (_user == null)
                return null;

            // Create a MembershipUser mock
            var mock = new Mock<MembershipUser>();
            mock.Setup(x => x.ProviderUserKey).Returns(_user.Id);

            return mock.Object;
        }
    }
}
