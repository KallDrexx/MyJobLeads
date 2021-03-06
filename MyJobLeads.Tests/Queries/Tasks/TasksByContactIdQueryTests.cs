﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;

namespace MyJobLeads.Tests.Queries.Tasks
{
    [TestClass]
    public class TasksByContactIdQueryTests : EFTestBase
    {
        private Contact _contact1, _contact2;
        private Task _task1, _task2, _task3;

        private void InitializeTestEntities()
        {
            _contact1 = new Contact { Tasks = new List<Task>() };
            _contact2 = new Contact { Tasks = new List<Task>() };

            _task1 = new Task();
            _task2 = new Task();
            _task3 = new Task();

            _contact1.Tasks.Add(_task1);
            _contact2.Tasks.Add(_task2);
            _contact1.Tasks.Add(_task3);

            _unitOfWork.Contacts.Add(_contact1);
            _unitOfWork.Contacts.Add(_contact2);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Tasks_For_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            IList<Task> results = new TasksByContactIdQuery(_unitOfWork).WithContactId(_contact1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(_task1.Id, results[0].Id, "The first task had an incorrect id value");
            Assert.AreEqual(_task3.Id, results[1].Id, "The second task had an incorrect id value");
        }
    }
}
