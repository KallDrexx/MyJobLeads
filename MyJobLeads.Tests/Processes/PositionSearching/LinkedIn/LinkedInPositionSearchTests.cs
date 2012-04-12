using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.PositionSearching;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Enums;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Exceptions.OAuth;
using Moq;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.Commands.Companies;

namespace MyJobLeads.Tests.Processes.PositionSearching.LinkedIn
{
    [TestClass]
    public class LinkedInPositionSearchTests : EFTestBase
    {
        protected IProcess<LinkedInPositionSearchParams, PositionSearchResultsViewModel> _process;
        protected Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>> _verifyTokenProcMock;
        protected User _user;
        protected Mock<IProcess<CreatePositionParams, PositionDisplayViewModel>> _createPositionMock;

        [TestInitialize]
        public void InitTests()
        {
            _verifyTokenProcMock = new Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>>();
            _verifyTokenProcMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = true });

            _createPositionMock = new Mock<IProcess<CreatePositionParams, PositionDisplayViewModel>>();
            var createCompanyCmd = new CreateCompanyCommand(_serviceFactory.Object);

            _process = new LinkedInPositionSearchProcesses(_context, _verifyTokenProcMock.Object, createCompanyCmd, _createPositionMock.Object, null);

            // Initialize user with test (but valid) access token data
            _user = new User();
            var oauth = new OAuthData
            {
                Token = "bfcf3fe4-b4d4-4f37-9d32-292ae9d45347",
                Secret = "f66673b2-5877-4fbf-80e0-3826ca9f7eed",
                TokenProvider = TokenProvider.LinkedIn,
                TokenType = TokenType.AccessToken,
            };

            oauth.LinkedInUsers.Add(_user);
            _user.LinkedInOAuthData = oauth;
            _context.Users.Add(_user);
            _context.OAuthData.Add(oauth);
            _context.SaveChanges();
        }

        [TestMethod]
        public void Can_Retrieve_Positions_From_LinkedIn()
        {
            // Act
            var result = _process.Execute(new LinkedInPositionSearchParams
            {
                RequestingUserId = _user.Id,
                Keywords = "Software Engineer",
                CountryCode = "us",
                ResultsPageNum = 2
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(2, result.PageNum, "The view model's page number was incorrect");
            Assert.IsNotNull(result.Results, "The result list was null");
            Assert.AreEqual(result.PageSize, result.Results.Count, "Result list had an incorrect number of results");
            Assert.AreEqual(ExternalDataSource.LinkedIn, result.DataSource, "Result had an incorrect position data source value");
        }

        [TestMethod]
        public void Throws_UserHasNoValidOAuthAccessTokenException_When_User_OAuth_Invalid()
        {
            // Setup
            _verifyTokenProcMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = false });

            // Act
            try
            {
                _process.Execute(new LinkedInPositionSearchParams
                {
                    RequestingUserId = _user.Id,
                    Keywords = "Software Engineer",
                    CountryCode = "us",
                    ZipCode = "32804",
                    ResultsPageNum = 2
                });
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
                _process.Execute(new LinkedInPositionSearchParams
                {
                    RequestingUserId = id,
                    Keywords = "Software Engineer",
                    CountryCode = "us",
                    ZipCode = "32804",
                    ResultsPageNum = 2
                });
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
