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

namespace MyJobLeads.Tests.Processes.Admin
{
    [TestClass]
    public class CreateOrgWithAdminUserTests : EFTestBase
    {
        protected const string _hashedPassword = "hashedPassword";
        protected Mock<IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel>> _genPassMock;

        [TestInitialize]
        public void SetupMocks()
        {
            _genPassMock = new Mock<IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel>>();
            _genPassMock.Setup(x => x.Execute(It.IsAny<GenerateUserPasswordHashParams>())).Returns(new GeneratedPasswordHashViewModel { PasswordHash = _hashedPassword });
        }

        [TestMethod]
        public void Can_Create_Organization_With_Administering_User()
        {
            // Setup
            IProcess<CreateOrgWithAdminUserParams, GeneralSuccessResultViewModel> process = new OrganizationAdminProcesses(_context, _genPassMock.Object);
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
        }
    }
}
