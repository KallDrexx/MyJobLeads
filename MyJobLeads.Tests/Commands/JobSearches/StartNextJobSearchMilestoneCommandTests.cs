using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Entities.Metrics;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.JobSearches;
using MyJobLeads.DomainModel.Queries.JobSearches;
using Moq;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.JobSearches
{
    [TestClass]
    public class StartNextJobSearchMilestoneCommandTests : EFTestBase
    {
        [TestMethod]
        public void Jobsearch_CurrentMilestone_Changes_To_Next_Milestone_When_Milestone_Completed()
        {
            // Setup
            MilestoneConfig ms2 = new MilestoneConfig();
            MilestoneConfig ms1 = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumCompaniesCreated = 2 }, NextMilestone = ms2 };
            JobSearch search = new JobSearch { Metrics = new JobSearchMetrics { NumCompaniesCreated = 2 }, CurrentMilestone = ms1 };

            _unitOfWork.JobSearches.Add(search);
            _unitOfWork.Commit();

            Mock<JobSearchByIdQuery> jobSearchQuery = new Mock<JobSearchByIdQuery>(_unitOfWork);
            jobSearchQuery.Setup(x => x.WithJobSearchId(It.IsAny<int>())).Returns(jobSearchQuery.Object);
            jobSearchQuery.Setup(x => x.Execute()).Returns(search);
            _serviceFactory.Setup(x => x.GetService<JobSearchByIdQuery>()).Returns(jobSearchQuery.Object);

            // Act
            new StartNextJobSearchMilestoneCommand(_serviceFactory.Object)
                .Execute(new StartNextJobSearchMilestoneCommandParams { JobSearchId = search.Id });

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual(ms2, result.CurrentMilestone, "Job search's CurrentMilestone was incorrect");
        }

        [TestMethod]
        public void Uncompleted_Jobsearch_Milestone_Doesnt_Progress_To_Next_Milestone()
        {
            // Setup
            MilestoneConfig ms2 = new MilestoneConfig();
            MilestoneConfig ms1 = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumCompaniesCreated = 2 }, NextMilestone = ms2 };
            JobSearch search = new JobSearch { Metrics = new JobSearchMetrics { NumCompaniesCreated = 1 }, CurrentMilestone = ms1 };

            _unitOfWork.JobSearches.Add(search);
            _unitOfWork.Commit();

            Mock<JobSearchByIdQuery> jobSearchQuery = new Mock<JobSearchByIdQuery>(_unitOfWork);
            jobSearchQuery.Setup(x => x.WithJobSearchId(It.IsAny<int>())).Returns(jobSearchQuery.Object);
            jobSearchQuery.Setup(x => x.Execute()).Returns(search);
            _serviceFactory.Setup(x => x.GetService<JobSearchByIdQuery>()).Returns(jobSearchQuery.Object);

            // Act
            new StartNextJobSearchMilestoneCommand(_serviceFactory.Object)
                .Execute(new StartNextJobSearchMilestoneCommandParams { JobSearchId = search.Id });

            // Verify
            JobSearch result = _unitOfWork.JobSearches.Fetch().Single();
            Assert.AreEqual(ms1, result.CurrentMilestone, "Job search's CurrentMilestone was incorrect");
        }

        [TestMethod]
        public void MJLEntityNotFoundException_Thrown_When_Jobsearch_Not_Found()
        {
            // Setup
            Mock<JobSearchByIdQuery> jobSearchQuery = new Mock<JobSearchByIdQuery>(_unitOfWork);
            jobSearchQuery.Setup(x => x.WithJobSearchId(It.IsAny<int>())).Returns(jobSearchQuery.Object);
            jobSearchQuery.Setup(x => x.Execute()).Returns((JobSearch)null);
            _serviceFactory.Setup(x => x.GetService<JobSearchByIdQuery>()).Returns(jobSearchQuery.Object);

            // Act
            try
            {
                new StartNextJobSearchMilestoneCommand(_serviceFactory.Object).Execute(new StartNextJobSearchMilestoneCommandParams { JobSearchId = 1 });
                Assert.Fail("No exception was thrown but one was expected");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(JobSearch), ex.EntityType, "MJLEntityNotFoundException's EntityType value was incorrect");
                Assert.AreEqual(1.ToString(), ex.IdValue, "MJLEntityNotFoundException's IdValue value was incorrect");
            }
        }
    }
}
