using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Queries.JobSearches
{
    [TestClass]
    public class JobSearchesByUserIdQueryTests : EFTestBase
    {
        private User _user1, _user2;
        private JobSearch _search1, _search2, _search3;

        private void InitializeTestEntities()
        {
            _user1 = new User { JobSearches = new List<JobSearch>() };
            _user2 = new User { JobSearches = new List<JobSearch>() };
            _search1 = new JobSearch();
            _search2 = new JobSearch();
            _search3 = new JobSearch();

            _user1.JobSearches.Add(_search1);
            _user2.JobSearches.Add(_search2);
            _user1.JobSearches.Add(_search3);

            _unitOfWork.Users.Add(_user1);
            _unitOfWork.Users.Add(_user2);
            _unitOfWork.JobSearches.Add(_search1);
            _unitOfWork.JobSearches.Add(_search2);
            _unitOfWork.JobSearches.Add(_search3);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Users_JobSearches()
        {
            // Setup
            InitializeTestEntities();

            // Act
            IList<JobSearch> results = new JobSearchesByUserIdQuery(_unitOfWork).WithUserId(_user1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(_search1.Id, results[0].Id, "First returned job search had an incorrect id value");
            Assert.AreEqual(_search3.Id, results[1].Id, "Second returned job search had an incorrect id value");
        }
    }
}
