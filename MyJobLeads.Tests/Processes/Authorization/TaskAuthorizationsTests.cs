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
    public class TaskAuthorizationsTests : EFTestBase
    {
        [TestMethod]
        public void User_Authorized_When_Task_Belongs_To_User_Jobsearch()
        {
            // Setup
            var user = new User();
            var task = new Task { Company = new Company { JobSearch = new JobSearch { User = user } } };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            IProcess<TaskAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            AuthorizationResultViewModel result = process.Execute(new TaskAuthorizationParams
            {
                TaskId = task.Id,
                RequestingUserId = user.Id
            });

            // Verify
            Assert.IsNotNull(result, "Result was null");
            Assert.IsTrue(result.UserAuthorized, "User was not authorized");
        }

        [TestMethod]
        public void User_Not_Authorized_When_Task__Doesnt_Belong_To_User_Jobsearch()
        {
            // Setup
            var user = new User();
            var user2 = new User();
            var task = new Task { Company = new Company { JobSearch = new JobSearch { User = user } } };
            var task2 = new Task { Company = new Company { JobSearch = new JobSearch { User = user2 } } };

            _context.Tasks.Add(task);
            _context.Tasks.Add(task2);
            _context.SaveChanges();

            IProcess<TaskAuthorizationParams, AuthorizationResultViewModel> process = new AuthorizationProcesses(_context);

            // Act
            AuthorizationResultViewModel result = process.Execute(new TaskAuthorizationParams
            {
                TaskId = task.Id,
                RequestingUserId = user2.Id
            });

            // Verify
            Assert.IsNotNull(result, "Result was null");
            Assert.IsFalse(result.UserAuthorized, "User was incorrectly authorized");
        }
    }
}
