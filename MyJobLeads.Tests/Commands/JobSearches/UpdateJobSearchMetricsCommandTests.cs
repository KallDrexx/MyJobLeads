using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.JobSearches;
using MyJobLeads.DomainModel;
using System;
using MyJobLeads.DomainModel.Providers;
using Moq;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads.Tests.Commands.JobSearches
{
    [TestClass]
    public class UpdateJobSearchMetricsCommandTests : EFTestBase
    {
        private JobSearch _search;

        private void InitializeTestEntities()
        {
            _search = new JobSearch();
            _unitOfWork.JobSearches.Add(_search);
            _unitOfWork.Commit();

            // Mocks
            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);
        }

        [TestMethod]
        public void Command_Sums_Created_Companies()
        {
            // Setup
            InitializeTestEntities();
            _unitOfWork.Companies.Add(new Company { JobSearch = _search });
            _unitOfWork.Companies.Add(new Company { JobSearch = new JobSearch() });
            _unitOfWork.Companies.Add(new Company { JobSearch = _search });
            _unitOfWork.Commit();

            // Act
            new UpdateJobSearchMetricsCommand(_serviceFactory.Object).Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = _search.Id });

            // Verify
            Assert.AreEqual(2, _search.Metrics.NumCompaniesCreated, "Number of companies created was incorrect");
        }

        [TestMethod]
        public void Command_Sums_Number_Of_Contacts()
        {
            // Setup
            InitializeTestEntities();
            _unitOfWork.Contacts.Add(new Contact { Company = new Company { JobSearch = _search } });
            _unitOfWork.Contacts.Add(new Contact { Company = new Company { JobSearch = new JobSearch() } });
            _unitOfWork.Contacts.Add(new Contact { Company = new Company { JobSearch = _search } });
            _unitOfWork.Commit();

            // Act
            new UpdateJobSearchMetricsCommand(_serviceFactory.Object).Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = _search.Id });

            // Verify
            Assert.AreEqual(2, _search.Metrics.NumContactsCreated, "Number of companies created was incorrect");
        }

        [TestMethod]
        public void Command_Sums_Number_Of_Application_Tasks()
        {
            // Setup
            InitializeTestEntities();
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.ApplyToFirmTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = new JobSearch() }, Category = MJLConstants.ApplyToFirmTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.ApplyToFirmTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.OtherTaskCategory });
            _unitOfWork.Commit();

            // Act
            new UpdateJobSearchMetricsCommand(_serviceFactory.Object).Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = _search.Id });

            // Verify
            Assert.AreEqual(2, _search.Metrics.NumApplyTasksCreated, "Number of application tasks created was incorrect");
        }

        [TestMethod]
        public void Command_Sums_Number_Of_Application_Tasks_Created()
        {
            // Setup
            InitializeTestEntities();
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.ApplyToFirmTaskCategory, CompletionDate = DateTime.Now });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = new JobSearch() }, Category = MJLConstants.ApplyToFirmTaskCategory, CompletionDate = DateTime.Now });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.ApplyToFirmTaskCategory, CompletionDate = DateTime.Now });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.OtherTaskCategory, CompletionDate = DateTime.Now });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.ApplyToFirmTaskCategory });
            _unitOfWork.Commit();

            // Act
            new UpdateJobSearchMetricsCommand(_serviceFactory.Object).Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = _search.Id });

            // Verify
            Assert.AreEqual(2, _search.Metrics.NumApplyTasksCompleted, "Number of application tasks completed was incorrect");
        }

        [TestMethod]
        public void Command_Sums_Number_Of_Phone_Interview_Tasks()
        {
            // Setup
            InitializeTestEntities();
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.PhoneInterviewTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = new JobSearch() }, Category = MJLConstants.PhoneInterviewTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.PhoneInterviewTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.OtherTaskCategory });
            _unitOfWork.Commit();

            // Act
            new UpdateJobSearchMetricsCommand(_serviceFactory.Object).Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = _search.Id });

            // Verify
            Assert.AreEqual(2, _search.Metrics.NumPhoneInterviewTasksCreated, "Number of phone interview tasks created was incorrect");
        }

        [TestMethod]
        public void Command_Sums_Number_Of_In_Person_Interview_Tasks()
        {
            // Setup
            InitializeTestEntities();
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.InPersonInterviewTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = new JobSearch() }, Category = MJLConstants.InPersonInterviewTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.InPersonInterviewTaskCategory });
            _unitOfWork.Tasks.Add(new Task { Company = new Company { JobSearch = _search }, Category = MJLConstants.OtherTaskCategory });
            _unitOfWork.Commit();

            // Act
            new UpdateJobSearchMetricsCommand(_serviceFactory.Object).Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = _search.Id });

            // Verify
            Assert.AreEqual(2, _search.Metrics.NumInPersonInterviewTasksCreated, "Number of in person interview tasks created was incorrect");
        }
    }
}
