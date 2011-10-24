using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.ProcessParams.JobSearches;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.JobSearches;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.JobSearches
{
    [TestClass]
    public class PropogateMilestoneToJobSearchTests : EFTestBase
    {
        [TestMethod]
        public void JobSearch_With_No_Current_Or_Finished_Milestones_Gets_Organizations_Starting_Milestone()
        {
            // Setup
            var org = new Organization();
            var search = new JobSearch { User = new User { Organization = org } };
            var ms = new MilestoneConfig { IsStartingMilestone = true };
            org.MilestoneConfigurations.Add(ms);

            _context.JobSearches.Add(search);
            _context.SaveChanges();

            IProcess<JobSearchMilestonePropogationParams, GeneralSuccessResultViewModel> process = new JobSearchMilestonePropogationProcesses(_context);

            // Verify
            var result = process.Execute(new JobSearchMilestonePropogationParams { JobSearchId = search.Id });

            // Act
            Assert.IsTrue(result.WasSuccessful, "Process did not return a successful result status");
            Assert.AreEqual(ms, search.CurrentMilestone, "Job search's current milestone is incorrect");
        }

        [TestMethod]
        public void Jobsearch_With_Current_Milestone_Does_Not_Get_Organization_Starting_Milestone_Propogated()
        {
            // Setup
            var org = new Organization();
            var search = new JobSearch { User = new User { Organization = org } };
            var ms1 = new MilestoneConfig { IsStartingMilestone = true };
            var ms2 = new MilestoneConfig { IsStartingMilestone = true };
            org.MilestoneConfigurations.Add(ms1);
            search.CurrentMilestone = ms2;

            _context.JobSearches.Add(search);
            _context.SaveChanges();

            IProcess<JobSearchMilestonePropogationParams, GeneralSuccessResultViewModel> process = new JobSearchMilestonePropogationProcesses(_context);

            // Verify
            var result = process.Execute(new JobSearchMilestonePropogationParams { JobSearchId = search.Id });

            // Act
            Assert.IsTrue(result.WasSuccessful, "Process did not return a successful result status");
            Assert.AreEqual(ms2, search.CurrentMilestone, "Job search's current milestone is incorrect");
        }

        [TestMethod]
        public void Jobsearch_That_Finished_Milestones_Already_Does_Not_Get_Organization_Starting_Milestone_Propogated()
        {
            // Setup
            var org = new Organization();
            var search = new JobSearch { User = new User { Organization = org }, MilestonesCompleted = true };
            var ms = new MilestoneConfig { IsStartingMilestone = true };
            org.MilestoneConfigurations.Add(ms);

            _context.JobSearches.Add(search);
            _context.SaveChanges();

            IProcess<JobSearchMilestonePropogationParams, GeneralSuccessResultViewModel> process = new JobSearchMilestonePropogationProcesses(_context);

            // Verify
            var result = process.Execute(new JobSearchMilestonePropogationParams { JobSearchId = search.Id });

            // Act
            Assert.IsTrue(result.WasSuccessful, "Process did not return a successful result status");
            Assert.IsNull(search.CurrentMilestone, "Job search's current milestone was not null");
        }

        [TestMethod]
        public void Throws_EntityNotFoundException_When_JobSearch_Doesnt_Exist()
        {
            // Setup
            int id = 105;
            IProcess<JobSearchMilestonePropogationParams, GeneralSuccessResultViewModel> process = new JobSearchMilestonePropogationProcesses(_context);

            // Act
            try
            {
                var result = process.Execute(new JobSearchMilestonePropogationParams { JobSearchId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(JobSearch), ex.EntityType, "Exception's entity type value was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
            }
        }
    }
}
