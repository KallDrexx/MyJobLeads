using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.JobSearches;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.JobSearches
{
    [TestClass]
    public class EditJobSearchCommandTests : EFTestBase
    {
        private JobSearch _jobSearch;

        private void InitializeTestEntities()
        {
            _jobSearch = new JobSearch { Name = "Test Name", Description = "Test Description" };
            _unitOfWork.JobSearches.Add(_jobSearch);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Edit_Job_Search_Properties()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                 .SetName("New Name")
                                                 .SetDescription("New Description")
                                                 .Execute();

            // Verify 
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual("New Name", result.Name, "The JobSearch had an incorrect name");
            Assert.AreEqual("New Description", result.Description, "The JobSearch had an incorrect description");
        }

        [TestMethod]
        public void Execute_Returns_Edited_JobSearch()
        {
            // Setup
            InitializeTestEntities();

            // Act
            JobSearch result = new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                                    .SetName("New Name")
                                                                    .SetDescription("New Description")
                                                                    .Execute();

            // Verify 
            Assert.IsNotNull(result, "Returned job search was null");
            Assert.AreEqual("New Name", result.Name, "The JobSearch had an incorrect name");
            Assert.AreEqual("New Description", result.Description, "The JobSearch had an incorrect description");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Jobsearch_Not_Found()
        {
            // Setup
            InitializeTestEntities();

            // Act
            try
            {
                new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id + 1)
                                                     .SetName("Name")
                                                     .SetDescription("Description")
                                                     .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(JobSearch), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual((_jobSearch.Id + 1).ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Doesnt_Change_Description_If_SetDescription_Not_Called()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                 .SetName("New Name")
                                                 .Execute();

            // Verify 
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual("New Name", result.Name, "The JobSearch had an incorrect name");
            Assert.AreEqual("Test Description", result.Description, "The JobSearch had an incorrect description");
        }

        [TestMethod]
        public void Execute_Doesnt_Change_Name_If_SetName_Not_Called()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                 .SetDescription("New Description")
                                                 .Execute();

            // Verify 
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual("Test Name", result.Name, "The JobSearch had an incorrect name");
            Assert.AreEqual("New Description", result.Description, "The JobSearch had an incorrect description");
        }
    }
}
