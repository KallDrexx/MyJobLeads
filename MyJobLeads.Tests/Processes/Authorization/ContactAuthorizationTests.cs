using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.Processes.Authorization;

namespace MyJobLeads.Tests.Processes.Authorization
{
    [TestClass]
    public class ContactAuthorizationTests : EFTestBase
    {
        [TestMethod]
        public void User_Authorized_When_Contact_Belongs_To_User_Jobsearch()
        {
            // Setup
            var user = new User();
            var search = new JobSearch { User = user };
            var company = new Company { JobSearch = search };
            var contact = new Contact { Company = company };

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            IProcess<ContactAutorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            AuthorizationResultViewModel result = process.Execute(new ContactAutorizationParams
            {
                ContactId = contact.Id,
                RequestingUserId = user.Id
            });

            // Verify
            Assert.IsNotNull(result, "Result was null");
            Assert.IsTrue(result.UserAuthorized, "User was not authorized");
        }

        [TestMethod]
        public void User_Not_Authorized_When_Contact_Doesnt_Belong_To_User_Jobsearch()
        {
            // Setup
            var user = new User();
            var user2 = new User();
            var search = new JobSearch { User = user };
            var company = new Company { JobSearch = search };
            var contact = new Contact { Company = company };

            _context.Contacts.Add(contact);
            _context.Users.Add(user2);
            _context.SaveChanges();

            IProcess<ContactAutorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            AuthorizationResultViewModel result = process.Execute(new ContactAutorizationParams
            {
                ContactId = contact.Id,
                RequestingUserId = user2.Id
            });

            // Verify
            Assert.IsNotNull(result, "Result was null");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }
    }
}
