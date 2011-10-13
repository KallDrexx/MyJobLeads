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
using MyJobLeads.DomainModel.Queries.MilestoneConfigs;
using Moq;
using MyJobLeads.DomainModel.Entities.Configuration;
using FluentValidation;
using FluentValidation.Results;

namespace MyJobLeads.Tests.Commands.JobSearches
{
    [TestClass]
    public class CreateJobSearchForUserCommandTests : EFTestBase
    {
        private User _user;
        private Organization _org;
        private Mock<StartingMilestoneQuery> _startingMilestoneQuery;
        private Mock<IValidator<JobSearch>> _validator;

        public void InitializeTestEntities()
        {
            _org = new Organization();
            _user = new User { JobSearches = new List<JobSearch>(), Organization = _org };
            _unitOfWork.Users.Add(_user);
            _unitOfWork.Commit();

            // Mocks
            _startingMilestoneQuery = new Mock<StartingMilestoneQuery>(_context);
            _serviceFactory.Setup(x => x.GetService<StartingMilestoneQuery>()).Returns(_startingMilestoneQuery.Object);

            _validator = new Mock<IValidator<JobSearch>>();
            _validator.Setup(x => x.Validate(It.IsAny<JobSearch>())).Returns(new ValidationResult());
            _serviceFactory.Setup(x => x.GetService<IValidator<JobSearch>>()).Returns(_validator.Object);
        }

        [TestMethod]
        public void Can_Create_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id)
                                                          .WithName("Test Name")
                                                          .WithDescription("Test Desc")
                                                          .Execute();

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().FirstOrDefault();
            Assert.IsNotNull(result, "No Jobsearch was created");
            Assert.AreEqual(_user.Id, result.User.Id, "Jobsearch had an incorrect user id value");
            Assert.AreEqual("Test Name", result.Name, "JobSearch had an incorrect name value");
            Assert.AreEqual("Test Desc", result.Description, "JobSearch had an incorrect description value");
            Assert.AreEqual(_user.Id, result.User.Id, "JobSearch had an incorrect user id value");
        }

        [TestMethod]
        public void Execute_Returns_Created_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            JobSearch result = new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id)
                                                                             .WithName("Test Name")
                                                                             .WithDescription("Test Desc")
                                                                             .Execute();

            // Verify
            Assert.IsNotNull(result, "No Jobsearch was returned");
            Assert.AreEqual(_user.Id, result.User.Id, "Jobsearch had an incorrect user id value");
            Assert.AreEqual("Test Name", result.Name, "JobSearch had an incorrect name value");
            Assert.AreEqual("Test Desc", result.Description, "JobSearch had an incorrect description value");
            Assert.AreEqual(_user.Id, result.User.Id, "JobSearch had an incorrect user id value");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();

            // Act
            try
            {
                new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id + 1)
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

        [TestMethod]
        public void Execute_Initializes_Company_List()
        {
            // Setup
            InitializeTestEntities();

            // Act
            JobSearch search = new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id).Execute();

            // Verify
            Assert.IsNotNull(search.Companies, "Company list was not initialized");
        }

        [TestMethod]
        public void Execute_Creates_History_Record()
        {
            // Setup
            InitializeTestEntities();
            DateTime start, end;

            // Act
            start = DateTime.Now;
            new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id)
                                                          .WithName("Test Name")
                                                          .WithDescription("Test Desc")
                                                          .Execute();
            end = DateTime.Now;

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            JobSearchHistory history = result.History.Single();

            Assert.AreEqual("Test Name", history.Name, "History record had an incorrect name value");
            Assert.AreEqual("Test Desc", history.Description, "History record had an incorrect description value");
            Assert.AreEqual(_user, history.AuthoringUser, "History record had an incorrect author");
            Assert.AreEqual(MJLConstants.HistoryInsert, history.HistoryAction, "History record had an incorrect history action value");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "History record had an incorrect modified date value");
        }

        [TestMethod]
        public void New_JobSearch_Has_Milestone_Set_To_Starting_MilestoneConfig()
        {
            // Setup
            InitializeTestEntities();
            MilestoneConfig config = new MilestoneConfig();
            _unitOfWork.MilestoneConfigs.Add(config);
            _unitOfWork.Commit();

            _startingMilestoneQuery.Setup(x => x.Execute(It.IsAny<StartingMilestoneQueryParams>())).Returns(config);

            // Act
            new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id).Execute();

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual(config, result.CurrentMilestone, "Job Search's current milestone was incorrect");
        }

        [TestMethod]
        public void Command_Calls_Validation_On_JobSearch()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id).Execute();

            // Verify
            _validator.Verify(x => x.Validate(It.IsAny<JobSearch>()), Times.Once());
        }

        [TestMethod]
        public void Command_Throws_ValidationException_When_Validation_Fails()
        {
            // Setup
            InitializeTestEntities();
            _validator.Setup(x => x.Validate(It.IsAny<JobSearch>()))
                        .Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("testp", "teste") }));

            // Act
            try
            {
                new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id).Execute();
                Assert.Fail("No exception occured");
            }

            // Verify
            catch (ValidationException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count(), "Incorrect number of validation errors returned");
                Assert.AreEqual("testp", ex.Errors.First().PropertyName, "The validation property name was incorrect");
                Assert.AreEqual("teste", ex.Errors.First().ErrorMessage, "The validation error message was incorrect");
            }
        }

        [TestMethod]
        public void New_JobSearch_Queries_For_Organizations_Starting_MilestoneConfig_When_User_In_Organization()
        {
            // Setup
            InitializeTestEntities();
            MilestoneConfig config = new MilestoneConfig();
            _unitOfWork.MilestoneConfigs.Add(config);
            _unitOfWork.Commit();

            _startingMilestoneQuery.Setup(x => x.Execute(It.Is<StartingMilestoneQueryParams>(y => y.OrganizationId == _org.Id))).Returns(config);

            // Act
            new CreateJobSearchForUserCommand(_serviceFactory.Object).ForUserId(_user.Id).Execute();

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual(config, result.CurrentMilestone, "Job Search's current milestone was incorrect");
        }
    }
}
