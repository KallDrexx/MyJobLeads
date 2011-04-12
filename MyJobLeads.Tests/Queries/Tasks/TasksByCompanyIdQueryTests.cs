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
        [TestMethod]
        public void Can_Retrieve_Tasks_For_Company()
        {
            // Setup
            var company1 = new Company { Tasks = new List<Task>() };
            var company2 = new Company { Tasks = new List<Task>() };

            var task1 = new Task();
            var task2 = new Task();
            var task3 = new Task();

            company1.Tasks.Add(task1);
            company2.Tasks.Add(task2);
            company1.Tasks.Add(task3);

            _unitOfWork.Companies.Add(company1);
            _unitOfWork.Companies.Add(company2);
            _unitOfWork.Commit();

            // Act
            IList<Task> results = new TasksByCompanyIdQuery(_unitOfWork).WithCompanyId(company1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(task1.Id, results[0].Id, "The first task had an incorrect id value");
            Assert.AreEqual(task3.Id, results[1].Id, "The second task had an incorrect id value");
        }

        [TestMethod]
        public void Query_Retrieves_Tasks_For_Contacts_In_Company()
        {
            // Setup
            var company1 = new Company { Contacts = new List<Contact>() };
            var company2 = new Company { Tasks = new List<Task>(), Contacts = new List<Contact>() };

            var contact1 = new Contact { Tasks = new List<Task>(), Company = company1 };
            var contact2 = new Contact { Tasks = new List<Task>(), Company = company2 };

            var task1 = new Task { Contact = contact1 };
            var task2 = new Task { Contact = contact2 };
            var task3 = new Task { Contact = contact1 };

            _unitOfWork.Tasks.Add(task1);
            _unitOfWork.Tasks.Add(task2);
            _unitOfWork.Tasks.Add(task3);
            _unitOfWork.Commit();

            // Act
            IList<Task> results = new TasksByCompanyIdQuery(_unitOfWork).WithCompanyId(company1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(task1.Id, results[0].Id, "The first task had an incorrect id value");
            Assert.AreEqual(task3.Id, results[1].Id, "The second task had an incorrect id value");
        }
    }
}
