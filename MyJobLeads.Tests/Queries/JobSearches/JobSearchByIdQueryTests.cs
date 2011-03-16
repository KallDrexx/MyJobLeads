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
    public class JobSearchByIdQueryTests : EFTestBase
    {
        private JobSearch _jobSearch;

        private void InitializeTestEntities()
        {
            _jobSearch = new JobSearch { Name = "Name2" };
            _unitOfWork.JobSearches.Add(new JobSearch { Name = "Name1" });
            _unitOfWork.JobSearches.Add(_jobSearch);
            _unitOfWork.JobSearches.Add(new JobSearch { Name = "Name3" });
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_JobSearch_By_Id()
        {
            // Setup
            InitializeTestEntities();

            // Act
            JobSearch result = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(_jobSearch.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Command returned a null job search");
            Assert.AreEqual(_jobSearch.Id, result.Id, "Returned job search had an incorrect id value");
            Assert.AreEqual(_jobSearch.Name, result.Name, "Returned job search had an incorrect id value");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_JobSearch_Not_Found()
        {
            // Setup 
            InitializeTestEntities();
            int id = _jobSearch.Id + 100;

            // Act
            try
            {
                JobSearch result = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(id).Execute();
                Assert.Fail("Query did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(JobSearch), ex.EntityType, "MJLEntityNotFoundException's entity type was not correct");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was not correct");
            }
        }
    }
}
