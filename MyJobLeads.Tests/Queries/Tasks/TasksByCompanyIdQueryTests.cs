using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;

namespace MyJobLeads.Tests.Queries.Tasks
{
    [TestClass]
    public class TasksByCompanyIdQueryTests : EFTestBase
    {
        private Company _company1, _company2;
        private Task _task1, _task2, _task3;

        private void InitializeTestEntities()
        {
            _company1 = new Company { Tasks = new List<Task>() };
            _company2 = new Company { Tasks = new List<Task>() };

            _task1 = new Task();
            _task2 = new Task();
            _task3 = new Task();

            _company1.Tasks.Add(_task1);
            _company2.Tasks.Add(_task2);
            _company1.Tasks.Add(_task3);

            _unitOfWork.Companies.Add(_company1);
            _unitOfWork.Companies.Add(_company2);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Tasks_For_Company()
        {
            // Setup
            InitializeTestEntities();

            // Act
            IList<Task> results = new TasksByCompanyIdQuery(_unitOfWork).WithCompanyId(_company1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(_task1.Id, results[0].Id, "The first task had an incorrect id value");
            Assert.AreEqual(_task3.Id, results[1].Id, "The second task had an incorrect id value");
        }
    }
}
