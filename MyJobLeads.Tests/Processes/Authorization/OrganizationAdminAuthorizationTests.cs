using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Authorization;

namespace MyJobLeads.Tests.Processes.Authorization
{
    [TestClass]
    public class OrganizationAdminAuthorizationTests : EFTestBase
    {
        [TestMethod]
        public void Returns_Successful_Authorization_When_User_Is_Admin_For_Specified_Organization()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new OrganizationAdminAuthorizationParams { OrganizationId = org.Id, UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.UserAuthorized, "User was not authorized");
        }

        [TestMethod]
        public void Returns_UnSuccessful_Authorization_When_User_Is_Admin_For_Different_Organization()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user = new User { Organization = org2, IsOrganizationAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new OrganizationAdminAuthorizationParams { OrganizationId = org1.Id, UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }

        [TestMethod]
        public void Returns_Unsuccessful_Authorization_When_User_Is_Not_Admin_For_Organization()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = false };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new OrganizationAdminAuthorizationParams { OrganizationId = org.Id, UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }

        [TestMethod]
        public void Returns_Unsuccessful_Authorization_When_User_Id_Doesnt_Exist()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new OrganizationAdminAuthorizationParams { OrganizationId = org.Id, UserId = user.Id + 5 });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }
    }
}
