using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Admin;
using MyJobLeads.DomainModel.ProcessParams.Users;
using MyJobLeads.DomainModel.ViewModels.Users;
using Moq;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.Admin
{
    [TestClass]
    public class CreateOrgWithAdminUserTests : EFTestBase
    {
        protected const string _hashedPassword = "hashedPassword";
        protected Mock<IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel>> _genPassMock;
        protected Mock<IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel>> _siteAdminMock;

        [TestInitialize]
        public void SetupMocks()
        {
            _genPassMock = new Mock<IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel>>();
            _genPassMock.Setup(x => x.Execute(It.IsAny<GenerateUserPasswordHashParams>())).Returns(new GeneratedPasswordHashViewModel { PasswordHash = _hashedPassword });

            _siteAdminMock = new Mock<IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel>>();
            _siteAdminMock.Setup(x => x.Execute(It.IsAny<SiteAdminAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });
        }

        [TestMethod]
        public void Can_Create_Organization_With_Administering_User()
        {
            // Setup
            IProcess<CreateOrgWithAdminUserParams, GeneralSuccessResultViewModel> process = new OrganizationAdminProcesses(_context, _genPassMock.Object, _siteAdminMock.Object);
            string email = "myemail", password = "pass", orgName = "my org", adminName = "the admin";

            // Act
            var result = process.Execute(new CreateOrgWithAdminUserParams 
            { 
                OrganizationName = orgName, 
                AdminEmail = email, 
                AdminName = adminName,
                AdminPlainTextPassword = password 
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.WasSuccessful, "Process did not return a successful result");

            var org = _context.Organizations.SingleOrDefault();
            Assert.IsNotNull(org, "No organization was created");
            Assert.AreEqual(orgName, org.Name, "Organization's name was incorrect");
            Assert.AreEqual(1, org.Members.Count, "Organization has a incorrect number of members");

            var user = org.Members.Single();
            Assert.AreEqual(email, user.Email, "User's email was incorrect");
            Assert.AreEqual(adminName, user.FullName, "User's full name was incorrect");
            Assert.AreEqual(_hashedPassword, user.Password, "User's password was incorrect");
            Assert.IsTrue(user.IsOrganizationAdmin, "User was not set as an organization admin");
        }

        [TestMethod]
        public void Throws_UserNotAuthorizedForProcessException_When_User_Not_Site_Admin()
        {
            // Setup
            IProcess<CreateOrgWithAdminUserParams, GeneralSuccessResultViewModel> process = new OrganizationAdminProcesses(_context, _genPassMock.Object, _siteAdminMock.Object);
            _siteAdminMock.Setup(x => x.Execute(It.IsAny<SiteAdminAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });

            // Act
            try
            {
                process.Execute(new CreateOrgWithAdminUserParams { RequestingUserId = 12 });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForProcessException ex)
            {
                Assert.AreEqual(12, ex.UserId, "Exception's user id value was incorrect");
                Assert.AreEqual(typeof(SiteAdminAuthorizationParams), ex.ProcessParamType, "Exception's process param type was incorrect");
                Assert.AreEqual(typeof(AuthorizationResultViewModel), ex.ViewModelType, "Exception's view model typewas incorrect");
            }
        }
    }
}
