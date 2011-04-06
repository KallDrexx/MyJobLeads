using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;

namespace MyJobLeads.Tests.Commands.Tasks
{
    [TestClass]
    public class CreateTaskForContactCommandTests : EFTestBase
    {
        private Contact _contact;
        private DateTime? _testDate;
        private User _user;

        private void InitializeTestEntities()
        {
            _contact = new Contact { Tasks = new List<Task>() };
            _user = new User();

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Contacts.Add(_contact);
            _unitOfWork.Commit();

            _testDate = new DateTime(2011, 1, 2, 3, 4, 5);
        }

        [TestMethod]
        public void Can_Create_Contact_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskForContactCommand(_unitOfWork).WithContactId(_contact.Id)
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
            Assert.AreEqual(_contact.Id, result.Contact.Id, "Task's contact id value is incorrect");
        }

        [TestMethod]
        public void Execute_Returns_Created_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new CreateTaskForContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                                        .SetName("Name")
                                                                        .SetTaskDate(_testDate)
                                                                        .RequestedByUserId(_user.Id)
                                                                        .Execute();

            // Verify
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_testDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsFalse(result.Completed, "Task's completed status value is incorrect");
            Assert.AreEqual(_contact.Id, result.Contact.Id, "Task's contact id value is incorrect");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Contact_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _contact.Id + 1;

            // Act
            try
            {
                new CreateTaskForContactCommand(_unitOfWork).WithContactId(id)
                                                            .SetName("Name")
                                                            .SetTaskDate(_testDate)
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
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 1;

            // Act
            try
            {
                new CreateTaskForContactCommand(_unitOfWork).WithContactId(_contact.Id)
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
            new CreateTaskForContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                        .SetName("Name")
                                                        .SetTaskDate(_testDate)
                                                        .RequestedByUserId(_user.Id)
                                                        .Execute();
            DateTime end = DateTime.Now;

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            TaskHistory history = task.History.Single();

            Assert.AreEqual("Name", history.Name, "The history record's name was incorrect");
            Assert.AreEqual(_testDate, history.TaskDate, "The history record's date value was incorrect");
            Assert.IsFalse(history.Completed, "The history record's completed status value was incorrect");
            Assert.AreEqual(_user, history.AuthoringUser, "The history record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryInsert, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
        }
    }
}
