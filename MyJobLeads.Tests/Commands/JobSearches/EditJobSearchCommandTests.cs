using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.JobSearches;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;

namespace MyJobLeads.Tests.Commands.JobSearches
{
    [TestClass]
    public class EditJobSearchCommandTests : EFTestBase
    {
        private JobSearch _jobSearch;
        private User _user;

        private void InitializeTestEntities()
        {
            _jobSearch = new JobSearch { Name = "Test Name", Description = "Test Description", History = new List<JobSearchHistory>() };
            _user = new User();

            _unitOfWork.Users.Add(_user);
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
                                                 .CalledByUserId(_user.Id)
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
                                                                    .CalledByUserId(_user.Id)
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
                                                     .CalledByUserId(_user.Id)
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
                                                 .CalledByUserId(_user.Id)
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
                                                 .CalledByUserId(_user.Id)
                                                 .Execute();

            // Verify 
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual("Test Name", result.Name, "The JobSearch had an incorrect name");
            Assert.AreEqual("New Description", result.Description, "The JobSearch had an incorrect description");
        }

        [TestMethod]
        public void Execute_Creates_History_Record()
        {
            // Setup
            InitializeTestEntities();

            // Act
            DateTime start = DateTime.Now;
            new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                 .SetName("New Name")
                                                 .SetDescription("New Description")
                                                 .CalledByUserId(_user.Id)
                                                 .Execute();
            DateTime end = DateTime.Now;

            // Verify 
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            JobSearchHistory history = result.History.Single();

            Assert.AreEqual("New Name", history.Name, "The history record had an incorrect name");
            Assert.AreEqual("New Description", history.Description, "The history record had an incorrect description");
            Assert.AreEqual(_user, history.AuthoringUser, "The history record had an incorrect author");
            Assert.AreEqual(MJLConstants.HistoryUpdate, history.HistoryAction, "The history record had an incorrect history action value");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record had an incorrect modification date");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 101;

            // Act
            try
            {
                new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                     .SetName("Name")
                                                     .SetDescription("Description")
                                                     .CalledByUserId(id)
                                                     .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Can_Add_To_Hidden_Company_Status_List()
        {
            // Setup
            InitializeTestEntities();
            _jobSearch.HiddenCompanyStatuses = "status1;status2;";
            _unitOfWork.Commit();

            // Act
            new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                 .HideCompanyStatus("new1")
                                                 .HideCompanyStatus("new2")
                                                 .CalledByUserId(_user.Id)
                                                 .Execute();
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();

            // Verify
            Assert.AreEqual("status1;status2;new1;new2;", result.HiddenCompanyStatuses, "Hidden company statuses value was incorrect");
        }

        [TestMethod]
        public void Can_Replace_Hidden_Company_Status_List()
        {
            // Setup
            InitializeTestEntities();
            _jobSearch.HiddenCompanyStatuses = "status1;status2;";
            _unitOfWork.Commit();

            // Act
            new EditJobSearchCommand(_unitOfWork).WithJobSearchId(_jobSearch.Id)
                                                 .ResetHiddenCompanyStatusList()
                                                 .HideCompanyStatus("new1")
                                                 .HideCompanyStatus("new2")
                                                 .CalledByUserId(_user.Id)
                                                 .Execute();
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();

            // Verify
            Assert.AreEqual("new1;new2;", result.HiddenCompanyStatuses, "Hidden company statuses value was incorrect");
        }
    }
}
