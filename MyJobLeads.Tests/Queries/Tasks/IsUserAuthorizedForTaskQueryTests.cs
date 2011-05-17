using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;

namespace MyJobLeads.Tests.Queries.Tasks
{
    [TestClass]
    public class IsUserAuthorizedForTaskQueryTests : EFTestBase
    {
        [TestMethod]
        public void User_Associated_With_Tasks_JobSearch_Is_Authorized()
        {
            // Setup
            User user = new User();
            JobSearch jobSearch = new JobSearch { User = user };
            Company company = new Company { JobSearch = jobSearch };
            Task task = new Task { Company = company };

            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Commit();

            // Act
            bool result = new IsUserAuthorizedForTaskQuery(_unitOfWork).WithUserId(user.Id).WithTaskId(task.Id).Execute();

            // Verify
            Assert.IsTrue(result, "User was incorrectly not authorized for the task");
        }

        [TestMethod]
        public void User_Not_Associated_With_Tasks_JobSearch_Is_Not_Authorized()
        {
            // Setup
            User user = new User(), user2 = new User();
            JobSearch jobSearch = new JobSearch { User = user };
            Company company = new Company { JobSearch = jobSearch };
            Task task = new Task { Company = company };

            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Users.Add(user2);
            _unitOfWork.Commit();

            // Act
            bool result = new IsUserAuthorizedForTaskQuery(_unitOfWork).WithUserId(user2.Id).WithTaskId(task.Id).Execute();

            // Verify
            Assert.IsFalse(result, "User was incorrectly authorized for the contact");
        }
    }
}
