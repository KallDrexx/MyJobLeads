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
    public class CreateJobSearchForUserCommandTests : EFTestBase
    {
        private User _user;

        public void InitializeTestEntities()
        {
            _user = new User { JobSearches = new List<JobSearch>() };
            _unitOfWork.Users.Add(_user);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Create_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateJobSearchForUserCommand(_unitOfWork).ForUserId(_user.Id)
                                                          .WithName("Test Name")
                                                          .WithDescription("Test Desc")
                                                          .Execute();

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().FirstOrDefault();
            Assert.IsNotNull(result, "No Jobsearch was created");
            Assert.AreEqual(_user.Id, result.User.Id, "Jobsearch had an incorrect user id value");
            Assert.AreEqual("Test Name", result.Name, "JobSearch had an incorrect name value");
            Assert.AreEqual("Test Desc", result.Description, "JobSearch had an incorrect description value");
        }

        [TestMethod]
        public void Execute_Returns_Created_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            JobSearch result = new CreateJobSearchForUserCommand(_unitOfWork).ForUserId(_user.Id)
                                                                             .WithName("Test Name")
                                                                             .WithDescription("Test Desc")
                                                                             .Execute();

            // Verify
            Assert.IsNotNull(result, "No Jobsearch was returned");
            Assert.AreEqual(_user.Id, result.User.Id, "Jobsearch had an incorrect user id value");
            Assert.AreEqual("Test Name", result.Name, "JobSearch had an incorrect name value");
            Assert.AreEqual("Test Desc", result.Description, "JobSearch had an incorrect description value");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();

            // Act
            try
            {
                new CreateJobSearchForUserCommand(_unitOfWork).ForUserId(_user.Id + 1)
                                                              .WithName("Test Name")
                                                              .WithDescription("Test Desc")
                                                              .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual((_user.Id + 1).ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}
