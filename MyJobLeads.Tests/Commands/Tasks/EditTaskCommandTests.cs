using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;
using MyJobLeads.DomainModel.Providers.Search;
using Moq;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Commands.JobSearches;
using FluentValidation;
using FluentValidation.Results;

namespace MyJobLeads.Tests.Commands.Tasks
{
    [TestClass]
    public class EditTaskCommandTests : EFTestBase
    {
        private Task _task;
        private JobSearch _jobSearch;
        private DateTime? _startDate, _changedDate;
        private User _user;
        private Contact _contact;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<UserByIdQuery> _userQuery;
        private Mock<TaskByIdQuery> _taskQuery;
        private Mock<ContactByIdQuery> _contactQuery;
        private Mock<UpdateJobSearchMetricsCommand> _updateMetricsCmd;
        private Mock<IValidator<Task>> _validator;

        private void InitializeTestEntities()
        {
            _changedDate = new DateTime(2011, 1, 2, 3, 4, 5);
            _startDate = new DateTime(2011, 2, 3, 4, 5, 6);

            _jobSearch = new JobSearch();
            Company company = new Company { JobSearch = _jobSearch };
            _user = new User();
            _contact = new Contact { Company = company };

            _task = new Task
            {
                Name = "Starting Name",
                CompletionDate = null,
                TaskDate = _startDate,
                Category = "Starting Category",
                Company = company,
                Notes = "Starting Notes",
                History = new List<TaskHistory>()
            };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Tasks.Add(_task);
            _unitOfWork.Contacts.Add(_contact);
            _unitOfWork.Commit();

            // Mocks
            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);

            _searchProvider = new Mock<ISearchProvider>();
            _serviceFactory.Setup(x => x.GetService<ISearchProvider>()).Returns(_searchProvider.Object);

            _userQuery = new Mock<UserByIdQuery>(_unitOfWork);
            _userQuery.Setup(x => x.Execute()).Returns(_user);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(_userQuery.Object);

            _contactQuery = new Mock<ContactByIdQuery>(_unitOfWork);
            _contactQuery.Setup(x => x.Execute()).Returns(_contact);
            _serviceFactory.Setup(x => x.GetService<ContactByIdQuery>()).Returns(_contactQuery.Object);

            _taskQuery = new Mock<TaskByIdQuery>(_unitOfWork);
            _taskQuery.Setup(x => x.Execute()).Returns(_task);
            _serviceFactory.Setup(x => x.GetService<TaskByIdQuery>()).Returns(_taskQuery.Object);

            _updateMetricsCmd = new Mock<UpdateJobSearchMetricsCommand>(_serviceFactory.Object);
            _serviceFactory.Setup(x => x.GetService<UpdateJobSearchMetricsCommand>()).Returns(_updateMetricsCmd.Object);

            _validator = new Mock<IValidator<Task>>();
            _validator.Setup(x => x.Validate(It.IsAny<Task>())).Returns(new ValidationResult());
            _serviceFactory.Setup(x => x.GetService<IValidator<Task>>()).Returns(_validator.Object);
        }

        [TestMethod]
        public void Can_Edit_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams());

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");
        }

        [TestMethod]
        public void Execute_Returns_Edited_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams());

            // Verify
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual(_unitOfWork.Tasks.Fetch().Single(), result, "The returned task did not match the one in the database");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Task_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _task.Id + 1;
            _taskQuery.Setup(x => x.Execute()).Returns((Task)null);

            // Act
            try
            {
                new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
                {
                    TaskId = id
                });
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Task), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
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
                new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams { RequestingUserId = id });
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
            DateTime start = DateTime.Now;
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                Name = "Name",
                TaskDate = _changedDate,
                Completed = true,
                ContactId = _contact.Id,
                Category = "Category",
                RequestingUserId = _user.Id
            });
            DateTime end = DateTime.Now;

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            TaskHistory history = task.History.Single();

            Assert.AreEqual("Name", history.Name, "The history record's name was incorrect");
            Assert.AreEqual(_changedDate, history.TaskDate, "The history record's date value was incorrect");
            Assert.AreEqual(_task.CompletionDate, history.CompletionDate, "The history record's completion date value was incorrect");
            Assert.AreEqual(_user, history.AuthoringUser, "The history record's author was incorrect");
            Assert.AreEqual(_contact, history.Contact, "The history record had an incorrect contact");
            Assert.AreEqual("Category", history.Category, "The history record's category value was incorrect");
            Assert.AreEqual(MJLConstants.HistoryUpdate, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
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
                new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams { ContactId = id });
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
        public void Task_Contact_Is_Changed_When_Contact_Specified()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                ContactId = _contact.Id,
                RequestingUserId = _user.Id,
            });

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(_contact, task.Contact, "Task had an incorrect contact");
        }

        [TestMethod]
        public void Not_Specifying_Contact_Doesnt_Change_Task_Contact()
        {
            // Setup
            InitializeTestEntities();
            _task.Contact = _contact;
            _unitOfWork.Commit();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                RequestingUserId = _user.Id
            });

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual(_contact, task.Contact, "Task had an incorrect contact");
        }

        [TestMethod]
        public void Specifying_Contact_Id_Of_Less_Than_One_Sets_Task_Contact_To_Null()
        {
            // Setup
            InitializeTestEntities();
            _task.Contact = _contact;
            _unitOfWork.Commit();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                ContactId = 0,
                RequestingUserId = _user.Id
            });

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            Assert.IsNull(task.Contact, "Task's contact was not null");
        }

        [TestMethod]
        public void Execute_Indexes_Edited_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                RequestingUserId = _user.Id
            });

            // Verify
            _searchProvider.Verify(x => x.Index(_task), Times.Once());
        }

        [TestMethod]
        public void Setting_Task_As_Completed_Sets_Completion_Date_To_Current_DateTime()
        {
            // Setup
            InitializeTestEntities();

            // Act
            DateTime start = DateTime.Now;
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                Completed = true,
                RequestingUserId = _user.Id
            });
            DateTime end = DateTime.Now;

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result.CompletionDate, "Task's completion date was null");
            Assert.IsTrue(result.CompletionDate >= start && result.CompletionDate <= end, "Task's completion date was incorrect");
        }

        [TestMethod]
        public void Setting_Task_As_Not_Completed_Sets_Completion_Date_To_Null()
        {
            // Setup
            InitializeTestEntities();
            _task.CompletionDate = DateTime.Now;
            _unitOfWork.Commit();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                Completed = false,
                RequestingUserId = _user.Id
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNull(result.CompletionDate, "Task's completion date was not set to null");
        }

        [TestMethod]
        public void Task_CompletionDate_Not_Updated_When_Already_Set()
        {
            // Setup
            InitializeTestEntities();
            _task.CompletionDate = new DateTime(2011, 1, 1);
            _unitOfWork.Commit();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams
            {
                TaskId = _task.Id,
                Completed = true,
                RequestingUserId = _user.Id
            });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result.CompletionDate, "Task's completion date was null");
            Assert.AreEqual(result.CompletionDate, new DateTime(2011, 1, 1), "Task's completion date was incorrect");
        }

        [TestMethod]
        public void Command_Updates_JobSearch_Metrics_After_Edit()
        {
            // Setup
            InitializeTestEntities();    

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams { TaskId = _task.Id });

            // Verify
            _updateMetricsCmd.Verify(x => x.Execute(It.Is<UpdateJobSearchMetricsCmdParams>(y => y.JobSearchId == _jobSearch.Id)));
        }

        [TestMethod]
        public void Can_Change_Task_Notes()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams { Notes = "New Note" });

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.AreEqual("New Note", result.Notes, "The task's notes were incorrect");
        }

        [TestMethod]
        public void Command_Performs_Contact_Validation()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams());

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
                new EditTaskCommand(_serviceFactory.Object).Execute(new EditTaskCommandParams());
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
