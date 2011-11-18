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
    public class SiteAdminAuthorizationTests : EFTestBase
    {
        [TestMethod]
        public void Authorization_Passes_For_Site_Admin()
        {
            // Setup
            var user = new User { IsSiteAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new SiteAdminAuthorizationParams { UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.UserAuthorized, "User was not authorized");
        }

        [TestMethod]
        public void Authorization_Fails_For_Non_Site_Admin()
        {
            // Setup
            var user = new User { IsSiteAdmin = false };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new SiteAdminAuthorizationParams { UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }

        [TestMethod]
        public void Authorization_Fails_For_NonExistant_User()
        {
            // Setup
            var user = new User { IsSiteAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            var result = process.Execute(new SiteAdminAuthorizationParams { UserId = user.Id + 1});

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }
    }
}
