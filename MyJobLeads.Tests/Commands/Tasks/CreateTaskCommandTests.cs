using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;
using Moq;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Commands.JobSearches;
using FluentValidation;
using FluentValidation.Results;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.Tests.Queries;

namespace MyJobLeads.Tests.Commands.Tasks
{
    [TestClass]
    public class CreateTaskCommandTests : EFTestBase
    {
        private Company _company;
        private Contact _contact;
        private JobSearch _jobSearch;
        private DateTime? _testDate;
        private User _user;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<UserByIdQuery> _userQuery;
        private Mock<CompanyByIdQuery> _companyQuery;
        private Mock<ContactByIdQuery> _contactQuery;
        private Mock<UpdateJobSearchMetricsCommand> _updateMetricsCmd;
        private Mock<IValidator<Task>> _validator;
        private Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>> _companyAuthMock;
        private Mock<IProcess<ContactAutorizationParams, AuthorizationResultViewModel>> _contactAuthMock;

        private void InitializeTestEntities()
        {
            _jobSearch = new JobSearch();
            _company = new Company { Tasks = new List<Task>(), JobSearch = _jobSearch };
            _user = new User();
            _contact = new Contact { Company = _company };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Companies.Add(_company);
            _unitOfWork.Contacts.Add(_contact);
            _unitOfWork.Commit();

            _testDate = new DateTime(2011, 1, 2, 3, 4, 5);

            // Mocks
            _companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            _companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            _contactAuthMock = new Mock<IProcess<ContactAutorizationParams, AuthorizationResultViewModel>>();
            _contactAuthMock.Setup(x => x.Execute(It.IsAny<ContactAutorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);

            _searchProvider = new Mock<ISearchProvider>();
            _serviceFactory.Setup(x => x.GetService<ISearchProvider>()).Returns(_searchProvider.Object);

            _userQuery = new Mock<UserByIdQuery>(_unitOfWork);
            _userQuery.Setup(x => x.Execute()).Returns(_user);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(_userQuery.Object);

            _companyQuery = QueryTestUtils.GenerateCompanyByIdQueryMock();
            _companyQuery.Setup(x => x.Execute()).Returns(_company);
            _serviceFactory.Setup(x => x.GetService<CompanyByIdQuery>()).Returns(_companyQuery.Object);

            _contactQuery = new Mock<ContactByIdQuery>(_unitOfWork, _contactAuthMock.Object);
            _contactQuery.Setup(x => x.Execute()).Returns(_contact);
            _serviceFactory.Setup(x => x.GetService<ContactByIdQuery>()).Returns(_contactQuery.Object);

            _updateMetricsCmd = new Mock<UpdateJobSearchMetricsCommand>(_serviceFactory.Object);
            _serviceFactory.Setup(x => x.GetService<UpdateJobSearchMetricsCommand>()).Returns(_updateMetricsCmd.Object);

            _validator = new Mock<IValidator<Task>>();
            _validator.Setup(x => x.Validate(It.IsAny<Task>())).Returns(new ValidationResult());
            _serviceFactory.Setup(x => x.GetService<IValidator<Task>>()).Returns(_validator.Object);
        }

        [TestMethod]
        public void Can_Create_Company_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");
        }

        [TestMethod]
        public void Execute_Returns_Created_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id
            });

            // Verify
            Assert.IsNotNull(result, "No task was returned");
            Task task = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(result, task, "Returned task and task in database were not the same");
        }

        [TestMethod]
        public void Can_Set_Task_Name()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id,
                Name = "Test Task"
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual("Test Task", result.Name, "Task's name was incorrect");
        }

        [TestMethod]
        public void Can_Set_Task_Category()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id,
                Category = "Category"
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual("Category", result.Category, "Task's category was incorrect");
        }

        [TestMethod]
        public void Can_Set_Task_Date()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id,
                TaskDate = _testDate
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(_testDate, result.TaskDate, "Task's date was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Company_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _company.Id + 1;
            _companyQuery.Setup(x => x.Execute()).Returns((Company)null);

            // Act
            try
            {
                new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
                {
                    CompanyId = id,
                    RequestedUserId = _user.Id
                });
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Company), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 1;
            _userQuery.Setup(x => x.Execute()).Returns((User)null);

            // Act
            try
            {
                new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
                {
                    CompanyId = _company.Id,
                    RequestedUserId = id
                });
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
        public void Execute_Creates_History_Record()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id
            });

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(1, task.History.Count, "Task had an incorrect number of history records");
        }

        [TestMethod]
        public void Created_Task_Can_Be_Associated_With_A_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id,
                ContactId = _contact.Id
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(_contact, result.Contact, "Task's contact was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Contact_Specified_But_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _contact.Id + 1;
            _contactQuery.Setup(x => x.Execute()).Returns((Contact)null);

            // Act
            try
            {
                new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
                {
                    CompanyId = _company.Id,
                    RequestedUserId = _user.Id,
                    ContactId = id
                });
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Contact), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void New_Task_Has_Null_Contact_When_None_Specified()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.IsNull(result.Contact, "Task did not have a null contact");
        }

        [TestMethod]
        public void Execute_Indexes_Created_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task task = new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id
            });

            // Verify
            _searchProvider.Verify(x => x.Index(task), Times.Once());
        }

        [TestMethod]
        public void Command_Updates_JobSearch_Metrics()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams
            {
                CompanyId = _company.Id,
                RequestedUserId = _user.Id
            });

            // Verify
            _updateMetricsCmd.Verify(x => x.Execute(It.Is<UpdateJobSearchMetricsCmdParams>(y => y.JobSearchId == _jobSearch.Id)));
        }

        [TestMethod]
        public void Can_Set_Task_Notes()
        {
            // Setup
            InitializeTestEntities();
            string note = "This is a test note";

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams { Notes = note });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(note, result.Notes, "The task's note was incorrect");
        }

        [TestMethod]
        public void Command_Performs_Contact_Validation()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams());

            // Verify
            _validator.Verify(x => x.Validate(It.IsAny<Task>()), Times.Once());
        }

        [TestMethod]
        public void Command_Throws_ValidationException_On_Task_Validation_Failure()
        {
            // Setup
            InitializeTestEntities();
            _validator.Setup(x => x.Validate(It.IsAny<Task>()))
                        .Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("testp", "teste") }));

            // Act
            try
            {
                new CreateTaskCommand(_serviceFactory.Object).Execute(new CreateTaskCommandParams());
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (ValidationException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count(), "Incorrect number of errors returned");
                Assert.AreEqual("testp", ex.Errors.First().PropertyName, "The error's property name was incorrect");
                Assert.AreEqual("teste", ex.Errors.First().ErrorMessage, "The error's message was incorrect");
            }
        }
    }
}
