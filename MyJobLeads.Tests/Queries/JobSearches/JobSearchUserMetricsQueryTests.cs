using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Queries.JobSearches;

namespace MyJobLeads.Tests.Queries.JobSearches
{
    [TestClass]
    public class JobSearchUserMetricsQueryTests : EFTestBase
    {
        private JobSearch _jobSearch;

        private void InitializeEntities()
        {
            _jobSearch = new JobSearch();
            _unitOfWork.JobSearches.Add(_jobSearch);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Metrics_For_Job_Search()
        {
            // Setup
            InitializeEntities();

            // Act
            JobSearchUserMetrics result = new JobSearchUserMetricsQuery(_unitOfWork).WithJobSearchId(_jobSearch.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null job search metric structure");
        }

        [TestMethod]
        public void Can_Retrieve_Number_Of_Tasks_In_JobSearch()
        {
            // Setup
            InitializeEntities();
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = new JobSearch() } });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Commit();

            // Act
            JobSearchUserMetrics results = new JobSearchUserMetricsQuery(_unitOfWork).WithJobSearchId(_jobSearch.Id).Execute();

            // Verify
            Assert.AreEqual(2, results.NumTasks, "Number of tasks returned was incorrect");
        }

        [TestMethod]
        public void Can_Retrieve_Number_Of_Completed_Tasks_In_JobSearch()
        {
            // Setup
            InitializeEntities();
            _unitOfWork.Tasks.Add(new Task { CompletionDate = DateTime.Now, Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Tasks.Add(new Task { CompletionDate = DateTime.Now, Company = new Company { JobSearch = new JobSearch() } });
            _unitOfWork.Tasks.Add(new Task { CompletionDate = DateTime.Now, Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Commit();

            // Act
            JobSearchUserMetrics results = new JobSearchUserMetricsQuery(_unitOfWork).WithJobSearchId(_jobSearch.Id).Execute();

            // Verify
            Assert.AreEqual(2, results.NumTasksCompleted, "Number of completed tasks returned was incorrect");
        }

        [TestMethod]
        public void Can_Retrieve_Number_Of_Companies_In_JobSearch()
        {
            // Setup
            InitializeEntities();
            _unitOfWork.Companies.Add(new Company { JobSearch = _jobSearch });
            _unitOfWork.Companies.Add(new Company { JobSearch = new JobSearch() });
            _unitOfWork.Companies.Add(new Company { JobSearch = _jobSearch });
            _unitOfWork.Commit();

            // Act
            JobSearchUserMetrics results = new JobSearchUserMetricsQuery(_unitOfWork).WithJobSearchId(_jobSearch.Id).Execute();

            // Verify
            Assert.AreEqual(2, results.NumCompanies, "Number of companies returned was incorrect");
        }

        [TestMethod]
        public void Can_Retrieve_Number_Of_Contacts_In_JobSearch()
        {
            // Setup
            InitializeEntities();
            _unitOfWork.Contacts.Add(new Contact { Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Contacts.Add(new Contact { Company = new Company { JobSearch = new JobSearch() } });
            _unitOfWork.Contacts.Add(new Contact { Company = new Company { JobSearch = _jobSearch } });
            _unitOfWork.Commit();

            // Act
            JobSearchUserMetrics results = new JobSearchUserMetricsQuery(_unitOfWork).WithJobSearchId(_jobSearch.Id).Execute();

            // Verify
            Assert.AreEqual(2, results.NumCompanies, "Number of contacts returned was incorrect");
        }
    }
}
