using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Queries.Tasks
{
    [TestClass]
    public class TaskByIdQueryTests : EFTestBase
    {
        private Task _task1, _task2, _task3;

        private void InitializeTestEntities()
        {
            _task1 = new Task { Name = "Task 1" };
            _task2 = new Task { Name = "Task 2" };
            _task3 = new Task { Name = "Task 3" };

            _unitOfWork.Tasks.Add(_task1);
            _unitOfWork.Tasks.Add(_task2);
            _unitOfWork.Tasks.Add(_task3);

            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Get_Task_By_ID()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Task result = new TaskByIdQuery(_unitOfWork).WithTaskId(_task2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null task");
            Assert.AreEqual(_task2.Id, result.Id, "Returned task had an incorrect id value");
            Assert.AreEqual(_task2.Name, result.Name, "Returned task had an incorrect name value");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Task_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _task2.Id + 100;

            // Act
            try
            {
                new TaskByIdQuery(_unitOfWork).WithTaskId(id).Execute();
                Assert.Fail("Query did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Task), ex.EntityType, "MJLEntityNotFoundException's entity type was invalid");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was invalid");
            }
        }
    }
}
