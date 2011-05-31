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
    public class OpenTasksByJobSearchQueryTests : EFTestBase
    {
        private JobSearch _jobsearch1, _jobsearch2;
        private Contact _contact1, _contact2;
        private Company _company1, _company2;
        private Task _contactTask1, _contactTask2, _companyTask1, _companyTask2, _closedTask1, _closedTask2;

        private void InitializeTestEntities()
        {
            _jobsearch1 = new JobSearch { Companies = new List<Company>() };
            _jobsearch2 = new JobSearch { Companies = new List<Company>() };
            _company1 = new Company { Contacts = new List<Contact>(), Tasks = new List<Task>(), JobSearch = _jobsearch1 };
            _company2 = new Company { Contacts = new List<Contact>(), Tasks = new List<Task>(), JobSearch = _jobsearch2 };
            _contact1 = new Contact { Tasks = new List<Task>(), Company = _company1 };
            _contact2 = new Contact { Tasks = new List<Task>(), Company = _company2 };

            _contactTask1 = new Task { Contact = _contact1, CompletionDate = null, TaskDate = new DateTime(2011, 1, 2, 5, 4, 5) };
            _contactTask2 = new Task { Contact = _contact2, CompletionDate = null, TaskDate = new DateTime(2011, 1, 2, 6, 4, 5) };
            _companyTask1 = new Task { Company = _company1, CompletionDate = null, TaskDate = new DateTime(2011, 1, 2, 1, 4, 5) };
            _companyTask2 = new Task { Company = _company2, CompletionDate = null, TaskDate = new DateTime(2011, 1, 2, 2, 4, 5) };
            _closedTask1 = new Task { Company = _company1, CompletionDate = DateTime.Now, TaskDate = new DateTime(2011, 1, 2, 3, 4, 5) };
            _closedTask2 = new Task { Company = _company2, CompletionDate = DateTime.Now, TaskDate = new DateTime(2011, 1, 2, 4, 4, 5) };

            _unitOfWork.JobSearches.Add(_jobsearch1);
            _unitOfWork.JobSearches.Add(_jobsearch2);
            _unitOfWork.Companies.Add(_company1);
            _unitOfWork.Companies.Add(_company2);
            _unitOfWork.Contacts.Add(_contact1);
            _unitOfWork.Contacts.Add(_contact2);
            _unitOfWork.Tasks.Add(_contactTask1);
            _unitOfWork.Tasks.Add(_contactTask2);
            _unitOfWork.Tasks.Add(_companyTask1);
            _unitOfWork.Tasks.Add(_companyTask2);
            _unitOfWork.Tasks.Add(_closedTask1);
            _unitOfWork.Tasks.Add(_closedTask2);
            _unitOfWork.Commit();
            
        }

        [TestMethod]
        public void Can_Retrieve_Open_Tasks_For_Contacts_and_Companies_In_Job_Search_Sorted_By_TaskDate()
        {
            // Setup
            InitializeTestEntities();

            // Act
            IList<Task> results = new OpenTasksByJobSearchQuery(_unitOfWork).WithJobSearch(_jobsearch1.Id).Execute();

            // Assert
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned task list had an incorrect number of elements");
            Assert.AreEqual(_companyTask1, results[0], "First returned task was incorrect");
            Assert.AreEqual(_contactTask1, results[1], "Second returned task was incorrect");
        }
    }
}
