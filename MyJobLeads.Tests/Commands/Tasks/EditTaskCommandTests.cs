using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;

namespace MyJobLeads.Tests.Commands.Tasks
{
    [TestClass]
    public class EditTaskCommandTests : EFTestBase
    {
        private Task _task;
        private DateTime? _startDate, _changedDate;
        private User _user;

        private void InitializeTestEntities()
        {
            _changedDate = new DateTime(2011, 1, 2, 3, 4, 5);
            _startDate = new DateTime(2011, 2, 3, 4, 5, 6);
            _user = new User();

            _task = new Task
            {
                Name = "Starting Name",
                Completed = false,
                TaskDate = _startDate,
                History = new List<TaskHistory>()
            };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Tasks.Add(_task);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Create_Company_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                            .SetName("Name")
                                            .SetTaskDate(_changedDate)
                                            .SetCompleted(true)
                                            .RequestedByUserId(_user.Id)
                                            .Execute();

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_changedDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsTrue(result.Completed, "Task's completed status value is incorrect");
        }

        [TestMethod]
        public void Execute_Returns_Edited_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                                            .SetName("Name")
                                                            .SetTaskDate(_changedDate)
                                                            .SetCompleted(true)
                                                            .RequestedByUserId(_user.Id)
                                                            .Execute();

            // Verify
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_changedDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsTrue(result.Completed, "Task's completed status value is incorrect");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Task_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _task.Id + 1;

            // Act
            try
            {
                new EditTaskCommand(_unitOfWork).WithTaskId(id)
                                                            .SetName("Name")
                                                            .SetTaskDate(_changedDate)
                                                            .SetCompleted(true)
                                                            .RequestedByUserId(_user.Id)
                                                            .Execute();
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
        public void Not_Specifying_Name_Doesnt_Change_Name_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                            .SetTaskDate(_changedDate)
                                            .SetCompleted(true)
                                            .RequestedByUserId(_user.Id)
                                            .Execute();

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Starting Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_changedDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsTrue(result.Completed, "Task's completed status value is incorrect");
        }

        [TestMethod]
        public void Not_Specifying_TaskDate_Doesnt_Change_TaskDate_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                            .SetName("Name")
                                            .SetCompleted(true)
                                            .RequestedByUserId(_user.Id)
                                            .Execute();

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_startDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsTrue(result.Completed, "Task's completed status value is incorrect");
        }

        [TestMethod]
        public void Not_Specifying_Completed_Doesnt_Change_Completed_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                            .SetName("Name")
                                            .SetTaskDate(_changedDate)
                                            .RequestedByUserId(_user.Id)
                                            .Execute();

            // Verify
            Task result = _unitOfWork.Tasks.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No task was created in the database");
            Assert.AreEqual("Name", result.Name, "Task's name was incorrect");
            Assert.AreEqual(_changedDate, result.TaskDate, "Task's date value was incorrect");
            Assert.IsFalse(result.Completed, "Task's completed status value is incorrect");
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
                new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                                .SetName("Name")
                                                .SetTaskDate(_changedDate)
                                                .SetCompleted(true)
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
            new EditTaskCommand(_unitOfWork).WithTaskId(_task.Id)
                                            .SetName("Name")
                                            .SetTaskDate(_changedDate)
                                            .SetCompleted(true)
                                            .RequestedByUserId(_user.Id)
                                            .Execute();
            DateTime end = DateTime.Now;

            // Verify
            Task task = _unitOfWork.Tasks.Fetch().Single();
            TaskHistory history = task.History.Single();

            Assert.AreEqual("Name", history.Name, "The history record's name was incorrect");
            Assert.AreEqual(_changedDate, history.TaskDate, "The history record's date value was incorrect");
            Assert.IsTrue(history.Completed, "The history record's completed status value is incorrect");
            Assert.AreEqual(_user, history.Author, "The history record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryUpdate, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
        }
    }
}
