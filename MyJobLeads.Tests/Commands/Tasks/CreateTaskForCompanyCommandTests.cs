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
    public class CreateTaskForCompanyCommandTests : EFTestBase
    {
        private Company _company;
        private DateTime? _testDate;
        private User _user;

        private void InitializeTestEntities()
        {
            _company = new Company { Tasks = new List<Task>() };
            _user = new User();

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Companies.Add(_company);
            _unitOfWork.Commit();

            _testDate = new DateTime(2011, 1, 2, 3, 4, 5);
        }

        [TestMethod]
        public void Can_Create_Company_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateTaskForCompanyCommand(_unitOfWork).WithCompanyId(_company.Id)
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
        }

        [TestMethod]
        public void Execute_Returns_Created_Task()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new CreateTaskForCompanyCommand(_unitOfWork).WithCompanyId(_company.Id)
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
                new CreateTaskForCompanyCommand(_unitOfWork).WithCompanyId(id)
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
                new CreateTaskForCompanyCommand(_unitOfWork).WithCompanyId(_company.Id)
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
            new CreateTaskForCompanyCommand(_unitOfWork).WithCompanyId(_company.Id)
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
    }
}
