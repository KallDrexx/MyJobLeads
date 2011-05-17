using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;

namespace MyJobLeads.Tests.Queries.JobSearches
{
    [TestClass]
    public class IsUserAuthorizedForJobSearchQueryTests : EFTestBase
    {
        [TestMethod]
        public void User_Associated_With_Contacts_JobSearch_Is_Authorized()
        {
            // Setup
            User user = new User();
            JobSearch jobSearch = new JobSearch { User = user };

            _unitOfWork.JobSearches.Add(jobSearch);
            _unitOfWork.Commit();

            // Act
            bool result = new IsUserAuthorizedForJobSearchQuery(_unitOfWork).WithUserId(user.Id).WithJobSearchId(jobSearch.Id).Execute();

            // Verify
            Assert.IsTrue(result, "User was incorrectly not authorized for the job search");
        }

        [TestMethod]
        public void User_Not_Associated_With_Contacts_JobSearch_Is_Not_Authorized()
        {
            // Setup
            User user = new User(), user2 = new User();
            JobSearch jobSearch = new JobSearch { User = user };

            _unitOfWork.Users.Add(user2);
            _unitOfWork.JobSearches.Add(jobSearch);
            _unitOfWork.Commit();

            // Act
            bool result = new IsUserAuthorizedForJobSearchQuery(_unitOfWork).WithUserId(user2.Id).WithJobSearchId(jobSearch.Id).Execute();

            // Verify
            Assert.IsFalse(result, "User was incorrectly authorized for the job search");
        }
    }
}
