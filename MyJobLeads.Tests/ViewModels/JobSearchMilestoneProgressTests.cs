using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Entities.Metrics;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.Tests.ViewModels
{
    [TestClass]
    public class JobSearchMilestoneProgressTests
    {
        [TestMethod]
        public void Can_Calculate_NumCompaniesCreatedProgress()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumCompaniesCreated = 5 } };
            JobSearch jobSearch = new JobSearch 
            { 
                Metrics = new JobSearchMetrics { NumCompaniesCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(((decimal)2 / (decimal)5), result.NumCompaniesCreatedProgress, "NumCompaniesCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progess_Is_One_When_Milestone_NumCompaniesCreated_Is_Zero()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumCompaniesCreated = 0 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumCompaniesCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumCompaniesCreatedProgress, "NumCompaniesCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progress_Is_One_When_JobSearch_NumCompaniesCreated_Is_Greater_Than_Milestone()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumCompaniesCreated = 3 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumCompaniesCreated = 4 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumCompaniesCreatedProgress, "NumCompaniesCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Can_Calculate_NumContactsCreatedProgress()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumContactsCreated = 5 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumContactsCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(((decimal)2 / (decimal)5), result.NumContactsCreatedProgress, "NumContactsCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progess_Is_One_When_Milestone_NumContactsCreated_Is_Zero()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumContactsCreated = 0 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumContactsCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumContactsCreatedProgress, "NumContactsCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progress_Is_One_When_JobSearch_NumContactsCreated_Is_Greater_Than_Milestone()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumContactsCreated = 3 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumContactsCreated = 4 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumContactsCreatedProgress, "NumContactsCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Can_Calculate_NumApplyTasksCreatedProgress()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumApplyTasksCreated = 5 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumApplyTasksCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(((decimal)2 / (decimal)5), result.NumApplyTasksCreatedProgress, "NumApplyTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progess_Is_One_When_Milestone_NumApplyTasksCreated_Is_Zero()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumApplyTasksCreated = 0 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumApplyTasksCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumApplyTasksCreatedProgress, "NumApplyTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progress_Is_One_When_JobSearch_NumApplyTasksCreated_Is_Greater_Than_Milestone()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumApplyTasksCreated = 3 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumApplyTasksCreated = 4 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumApplyTasksCreatedProgress, "NumApplyTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Can_Calculate_NumApplyTasksCompletedProgress()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumApplyTasksCompleted = 5 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumApplyTasksCompleted = 2 },
                CurrentMilestone = milestone
            };

            // Execute
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(((decimal)2 / (decimal)5), result.NumApplyTasksCompletedProgress, "NumApplyTasksCompletedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progess_Is_One_When_Milestone_NumApplyTasksCompleted_Is_Zero()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumApplyTasksCompleted = 0 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumApplyTasksCompleted = 2 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumApplyTasksCompletedProgress, "NumApplyTasksCompletedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progress_Is_One_When_JobSearch_NumApplyTasksCompleted_Is_Greater_Than_Milestone()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumApplyTasksCompleted = 3 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumApplyTasksCompleted = 4 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumApplyTasksCompletedProgress, "NumApplyTasksCompletedProgress's value was incorrect");
        }

        [TestMethod]
        public void Can_Calculate_NumPhoneInterviewTasksCreatedProgress()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumPhoneInterviewTasksCreated = 5 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumPhoneInterviewTasksCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(((decimal)2 / (decimal)5), result.NumPhoneInterviewTasksCreatedProgress, "NumPhoneInterviewTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progess_Is_One_When_Milestone_NumPhoneInterviewTasksCreated_Is_Zero()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumPhoneInterviewTasksCreated = 0 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumPhoneInterviewTasksCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumPhoneInterviewTasksCreatedProgress, "NumPhoneInterviewTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progress_Is_One_When_JobSearch_NumPhoneInterviewTasksCreated_Is_Greater_Than_Milestone()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumPhoneInterviewTasksCreated = 3 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumPhoneInterviewTasksCreated = 4 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumPhoneInterviewTasksCreatedProgress, "NumPhoneInterviewTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Can_Calculate_NumInPersonInterviewTasksCreatedProgress()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumInPersonInterviewTasksCreated = 5 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumInPersonInterviewTasksCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(((decimal)2 / (decimal)5), result.NumInPersonInterviewTasksCreatedProgress, "NumInPersonInterviewTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progess_Is_One_When_Milestone_NumInPersonInterviewTasksCreated_Is_Zero()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumInPersonInterviewTasksCreated = 0 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumInPersonInterviewTasksCreated = 2 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumInPersonInterviewTasksCreatedProgress, "NumInPersonInterviewTasksCreatedProgress's value was incorrect");
        }

        [TestMethod]
        public void Progress_Is_One_When_JobSearch_NumInPersonInterviewTasksCreated_Is_Greater_Than_Milestone()
        {
            // Setup
            MilestoneConfig milestone = new MilestoneConfig { JobSearchMetrics = new JobSearchMetrics { NumInPersonInterviewTasksCreated = 3 } };
            JobSearch jobSearch = new JobSearch
            {
                Metrics = new JobSearchMetrics { NumInPersonInterviewTasksCreated = 4 },
                CurrentMilestone = milestone
            };

            // Execute 
            JobSearchMilestoneProgress result = new JobSearchMilestoneProgress(jobSearch);

            // Verify
            Assert.AreEqual(1, result.NumInPersonInterviewTasksCreatedProgress, "NumInPersonInterviewTasksCreatedProgress's value was incorrect");
        }
    }
}
