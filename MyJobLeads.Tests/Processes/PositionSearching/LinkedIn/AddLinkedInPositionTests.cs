using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using Moq;
using MyJobLeads.DomainModel.Processes.PositionSearching;
using MyJobLeads.DomainModel.Enums;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Exceptions.OAuth;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.PositionSearching.LinkedIn
{
    [TestClass]
    public class AddLinkedInPositionTests : EFTestBase
    {
        protected const string POSITION_ID = "217456";

        protected IProcess<AddLinkedInPositionParams, ExternalPositionAddedResultViewModel> _process;
        protected Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>> _verifyTokenMock;
        protected User _user;

        [TestInitialize]
        public void Init()
        {
            _verifyTokenMock = new Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>>();
            _verifyTokenMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = true });

            _process = new LinkedInPositionSearchProcesses(_context, _verifyTokenMock.Object);

            // Initialize user with test (but valid) access token data
            _user = new User { LastVisitedJobSearch = new JobSearch() };
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
        public void Can_Add_Position_And_Company_To_Users_Jobsearch()
        {
            // Act
            var result = _process.Execute(new AddLinkedInPositionParams { RequestingUserId = _user.Id, PositionId = POSITION_ID });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.AddedSuccessfully, "Process' result was not marked as successful");

            var company = _user.LastVisitedJobSearch.Companies.FirstOrDefault();
            Assert.IsNotNull(company, "No company was added for the user");
            Assert.AreEqual(result.CompanyId, company.Id, "Returned company ID did not match the company in the database");

            var position = company.Positions.FirstOrDefault();
            Assert.IsNotNull(position, "No position was added for the company");
            Assert.AreEqual(result.PositionId, position.Id, "Returned position id did not match the position in the database");
        }

        [TestMethod]
        public void Throws_UserHasNoValidOAuthAccessTokenException_When_User_OAuth_Invalid()
        {
            // Setup
            _verifyTokenMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = false });

            // Act
            try
            {
                _process.Execute(new AddLinkedInPositionParams { PositionId = POSITION_ID, RequestingUserId = _user.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserHasNoValidOAuthAccessTokenException ex)
            {
                Assert.AreEqual(_user.Id, ex.UserId, "Exception's user id value was incorrect");
                Assert.AreEqual(TokenProvider.LinkedIn, ex.TokenProvider, "Exception's token provider was incorrect");
            }
        }

        [TestMethod]
        public void Throws_EntityNotFoundException_When_User_Doesnt_Exist()
        {
            // Setup
            int id = _user.Id + 101;

            // Act
            try
            {
                _process.Execute(new AddLinkedInPositionParams { RequestingUserId = id, PositionId = POSITION_ID });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
                Assert.AreEqual(typeof(User), ex.EntityType, "Exception's entity type value was incorrect");
            }
        }
    }
}
