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
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.Commands.Companies;
using MyJobLeads.Tests.Commands;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.Tests.Queries;

namespace MyJobLeads.Tests.Processes.PositionSearching.LinkedIn
{
    [TestClass]
    public class AddLinkedInPositionTests : EFTestBase
    {
        protected const string POSITION_ID = "217456";
        protected const int NEW_COMPANY_ID = 55;

        protected IProcess<AddLinkedInPositionParams, ExternalPositionAddedResultViewModel> _process;
        protected Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>> _verifyTokenMock;
        protected User _user;
        protected Mock<IProcess<CreatePositionParams, PositionDisplayViewModel>> _createPositionMock;
        protected Mock<CreateCompanyCommand> _createCompanyCmdMock;
        protected Mock<CompanyByIdQuery> _companyQueryMock;

        [TestInitialize]
        public void Init()
        {
            _createPositionMock = new Mock<IProcess<CreatePositionParams, PositionDisplayViewModel>>();
            _createPositionMock.Setup(x => x.Execute(It.IsAny<CreatePositionParams>())).Returns(new PositionDisplayViewModel());

            _verifyTokenMock = new Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>>();
            _verifyTokenMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = true });

            _createCompanyCmdMock = CommandTestUtils.GenerateCreateCompanyCommandMock();
            _createCompanyCmdMock.Setup(x => x.Execute()).Returns(new Company { Id = NEW_COMPANY_ID });

            _companyQueryMock = QueryTestUtils.GenerateCompanyByIdQueryMock();
            _process = new LinkedInPositionSearchProcesses(_context, _verifyTokenMock.Object, _createCompanyCmdMock.Object, _createPositionMock.Object, _companyQueryMock.Object);

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
        public void Can_Add_Position()
        {
            // Setup
            string expTitle = "Research Manager supporting Forbes.com worldwide online ad sales team";

            // Act
            var result = _process.Execute(new AddLinkedInPositionParams
            {
                RequestingUserId = _user.Id,
                PositionId = POSITION_ID,
                CreateNewCompany = true
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            _createPositionMock.Verify(x => x.Execute(It.Is<CreatePositionParams>(y => y.Title == expTitle)));
            _createPositionMock.Verify(x => x.Execute(It.Is<CreatePositionParams>(y => y.RequestingUserId == _user.Id)));
            _createPositionMock.Verify(x => x.Execute(It.Is<CreatePositionParams>(y => y.CompanyId == NEW_COMPANY_ID)));
        }

        [TestMethod]
        public void Can_Add_Company_To_Users_Jobsearch()
        {
            // Act
            var result = _process.Execute(new AddLinkedInPositionParams 
            { 
                RequestingUserId = _user.Id, 
                PositionId = POSITION_ID,
                CreateNewCompany = true
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            _createCompanyCmdMock.Verify(x => x.SetName("Forbes.com Inc."));
            _createCompanyCmdMock.Verify(x => x.WithJobSearch(_user.LastVisitedJobSearch.Id));
        }

        [TestMethod]
        public void Can_Add_Position_To_Existing_Company()
        {
            // Setup
            _companyQueryMock.Setup(x => x.Execute()).Returns(new Company { Id = 12 });

            // Act
            var result = _process.Execute(new AddLinkedInPositionParams
            {
                PositionId = POSITION_ID,
                RequestingUserId = _user.Id,
                ExistingCompanyId = 23
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            _companyQueryMock.Verify(x => x.RequestedByUserId(_user.Id));
            _companyQueryMock.Verify(x => x.WithCompanyId(23));
            _createPositionMock.Verify(x => x.Execute(It.Is<CreatePositionParams>(y => y.CompanyId == 12)));
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
