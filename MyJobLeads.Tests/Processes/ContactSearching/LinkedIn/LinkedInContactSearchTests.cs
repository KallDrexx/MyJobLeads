using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.Data;
using Moq;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Enums;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Processes.ContactSearching;
using MyJobLeads.DomainModel.Exceptions.OAuth;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.ContactSearching.LinkedIn
{
    [TestClass]
    public class LinkedInContactSearchTests : EFTestBase
    {
        protected IProcess<LinkedInContactSearchParams, ExternalContactSearchResultsViewModel> _process;
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
        public void Can_Retrieve_LinkedIn_Contacts()
        {
            // Act
            var result = _process.Execute(new LinkedInContactSearchParams { FirstName = "Matthew", RequestingUserId = _user.Id });

            // Assert
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(0, result.DisplayedPageNumber, "Result's displayed page number is incorrect");
            Assert.IsTrue(result.TotalResultsCount > 0, "Total results count was not greater than zero");
            Assert.IsTrue(result.Results.Count > 0, "Total number of returned results was not greater than zero");
            Assert.AreEqual(10, result.PageSize, "Page size was incorrect");
        }

        [TestMethod]
        public void Throws_UserHasNoValidOAuthAccessTokenException_When_User_Doesnt_Have_Valid_OAuth_Token()
        {
            // Setup
            _verifyTokenProcMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = false });

            // Act
            try
            {
                _process.Execute(new LinkedInContactSearchParams { RequestingUserId = _user.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserHasNoValidOAuthAccessTokenException ex)
            {
                Assert.AreEqual(_user.Id, ex.UserId, "Exception's user id was incorrect");
            }
        }

        [TestMethod]
        public void Throws_MJLEntityNotFoundException_When_User_Doesnt_Exist()
        {
            // Setup
            int id = _user.Id + 101;

            // Act
            try
            {
                _process.Execute(new LinkedInContactSearchParams { RequestingUserId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
            }
        }
    }
}