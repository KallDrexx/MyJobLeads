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

namespace MyJobLeads.Tests.Commands.Tasks
{
    [TestClass]
    public class CreateTaskCommandTests : EFTestBase
    {
        private Company _company;
        private Contact _contact;
        private DateTime? _testDate;
        private User _user;

        private void InitializeTestEntities()
        {
            _company = new Company { Tasks = new List<Task>() };
            _user = new User();
            _contact = new Contact { Company = _company };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Companies.Add(_company);
            _unitOfWork.Contacts.Add(_contact);
            _unitOfWork.Commit();

            _testDate = new DateTime(2011, 1, 2, 3, 4, 5);
        }

        [TestMethod]
        public void Can_Create_Company_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                        .SetName("Name")
                                                        .SetTaskDate(_testDate)
                                                        .RequestedByUserId(_user.Id)
                                                        .Execute();

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");

            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_testDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsFalse(result.Completed, "Task's completed status value is incorrect");
            Assert.AreEqual(_company.Id, result.Company.Id, "Tasks' company id was incorrect");
            Assert.IsNull(result.Contact, "Tasks' contact was incorrectly set");
            Assert.AreEqual(_user, result.CreatedByUser, "The created task was associated with the incorrect created user");
        }

        [TestMethod]
        public void Execute_Returns_Created_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                                        .SetName("Name")
                                                                        .SetTaskDate(_testDate)
                                                                        .RequestedByUserId(_user.Id)
                                                                        .Execute();

            // Verify
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_testDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsFalse(result.Completed, "Task's completed status value is incorrect");
            Assert.AreEqual(_company.Id, result.Company.Id, "Tasks' company id was incorrect");
            Assert.AreEqual(_user, result.CreatedByUser, "The created task was associated with the incorrect created user");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Company_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _company.Id + 1;

            // Act
            try
            {
                new CreateTaskCommand(_unitOfWork).WithCompanyId(id)
                                                            .SetName("Name")
                                                            .SetTaskDate(_testDate)
                                                            .RequestedByUserId(_user.Id)
                                                            .Execute();
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

            // Act
            try
            {
                new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                            .SetName("Name")
                                                            .SetTaskDate(_testDate)
                                                            .RequestedByUserId(id)
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
        public void Execute_Creates_History_Record()
        {
            // Setup
            InitializeTestEntities();

            // Act
            DateTime start = DateTime.Now;
            new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                        .SetName("Name")
                                                        .SetTaskDate(_testDate)
                                                        .RequestedByUserId(_user.Id)
                                                        .Execute();
            DateTime end = DateTime.Now;

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            TaskHistory history = task.History.Single();

            Assert.AreEqual("Name", history.Name, "History Record's name was incorrect");
            Assert.AreEqual(_testDate, history.TaskDate, "History Record's date value was incorrect");
            Assert.AreEqual(task.Completed, history.Completed, "History Record's completed status value was incorrect");
            Assert.AreEqual(_user, history.AuthoringUser, "History Record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryInsert, history.HistoryAction, "History Record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "History Record's modification date was incorrect");
        }

        [TestMethod]
        public void Created_Task_Can_Be_Associated_With_A_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                        .WithContactId(_contact.Id)
                                                        .RequestedByUserId(_user.Id)
                                                        .Execute();

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

            // Act
            try
            {
                new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                            .SetName("Name")
                                                            .SetTaskDate(_testDate)
                                                            .WithContactId(id)
                                                            .RequestedByUserId(_user.Id)
                                                            .Execute();
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
            new CreateTaskCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                        .RequestedByUserId(_user.Id)
                                                        .Execute();

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().Single();
            Assert.IsNull(result.Contact, "Task did not have a null contact");
        }
    }
}
