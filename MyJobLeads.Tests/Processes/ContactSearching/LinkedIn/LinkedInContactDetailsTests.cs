using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using Moq;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Processes.ContactSearching;
using MyJobLeads.DomainModel.Enums;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace MyJobLeads.Tests.Processes.ContactSearching.LinkedIn
{
    [TestClass]
    public class LinkedInContactDetailsTests : EFTestBase
    {
        protected const string CONTACT_ID = "MAwmvIpxP5";

        protected IProcess<LinkedInContactDetailsParams, ExternalContactSearchResultsViewModel> _process;
        protected Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>> _verifyTokenProcMock;
        protected User _user;

        [TestInitialize]
        public void Init()
        {
            _verifyTokenProcMock = new Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>>();
            _verifyTokenProcMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = true });

            _process = new LinkedInContactSearchProcesses(_context, _verifyTokenProcMock.Object);

            // Initialize user with test (but valid) access token data
            _user = new User();
            var oauth = new OAuthData
            {
                Token = "bfcf3fe4-b4d4-4f37-9d32-292ae9d45347",
                Secret = "f66673b2-5877-4fbf-80e0-3826ca9f7eed",
                TokenProvider = TokenProvider.LinkedIn,
                TokenType = TokenType.AccessToken,
                LinkedInUser = _user
            };

            _user.LinkedInOAuthData = oauth;
            _context.Users.Add(_user);
            _context.OAuthData.Add(oauth);
            _context.SaveChanges();
        }

        [TestMethod]
        public void Can_Get_Contact_Details_From_LinkedIn()
        {
            // Act
            //var result = _process.Execute(new LinkedInContactDetailsParams { RequestingUserId = _user.Id, ContactId = CONTACT_ID });
            var result = _process.Execute(new LinkedInContactDetailsParams { RequestingUserId = _user.Id, PublicUrl = @"http://www.linkedin.com/in/matthewhakaim" });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
        }
    }
}
