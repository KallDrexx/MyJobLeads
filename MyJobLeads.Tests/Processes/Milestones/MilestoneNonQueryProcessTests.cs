using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Milestones;
using MyJobLeads.DomainModel.ViewModels.Milestones;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Milestones;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.Tests.Processes.Milestones
{
    [TestClass]
    public class MilestoneNonQueryProcessTests : EFTestBase
    {
        [TestMethod]
        public void Can_Create_New_Milestone_For_Organization_When_Id_Is_Zero()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();

            var param = new SaveMilestoneParams
            {
                RequestingUserId = user.Id,
                OrganizationId = org.Id,
                Title = "Title",
                Instructions = "Instructions",
                CompletionDisplay = "Completion",
                JobSearchMetrics = new DomainModel.Entities.Metrics.JobSearchMetrics
                {
                    NumApplyTasksCompleted = 1,
                    NumApplyTasksCreated = 2,
                    NumCompaniesCreated = 3,
                    NumContactsCreated = 4,
                    NumInPersonInterviewTasksCreated = 5,
                    NumPhoneInterviewTasksCreated = 6
                }
            };

            IProcess<SaveMilestoneParams, MilestoneIdViewModel> process = new MilestoneNonQueryProcesses(_context);

            // Act
            var result = process.Execute(param);

            // Verify
            var milestone = _context.MilestoneConfigs.SingleOrDefault();

            Assert.IsNotNull(milestone, "No milestone was created");
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(milestone.Id, result.Id, "Result's id value and the milestone's id value did not match");
            Assert.AreEqual(org, milestone.Organization, "Milestone's organization was incorrect");
            Assert.AreEqual(param.Title, milestone.Title, "Milestone's title was incorrect");
            Assert.AreEqual(param.Instructions, milestone.Instructions, "Milestone's instructions were incorrect");
            Assert.AreEqual(param.CompletionDisplay, milestone.CompletionDisplay, "Milestone's completion text was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumApplyTasksCompleted, milestone.JobSearchMetrics.NumApplyTasksCompleted, "Milestones num apply tasks completed was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumApplyTasksCreated, milestone.JobSearchMetrics.NumApplyTasksCreated, "Milestones num apply tasks created was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumCompaniesCreated, milestone.JobSearchMetrics.NumCompaniesCreated, "Milestones num companies was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumContactsCreated, milestone.JobSearchMetrics.NumContactsCreated, "Milestones num contacts was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumInPersonInterviewTasksCreated, milestone.JobSearchMetrics.NumInPersonInterviewTasksCreated, "Milestones num InPersonInterview Tasks completed was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumPhoneInterviewTasksCreated, milestone.JobSearchMetrics.NumPhoneInterviewTasksCreated, "Milestones Num InPersonInterview Tasks created was incorrect");
        }

        [TestMethod]
        public void Throws_UserNotAuthorizedForEntityException_When_Creating_Milestone_And_User_Not_Admin_For_Organization()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = false };
            _context.Users.Add(user);
            _context.SaveChanges();

            var param = new SaveMilestoneParams
            {
                RequestingUserId = user.Id,
                OrganizationId = org.Id
            };

            IProcess<SaveMilestoneParams, MilestoneIdViewModel> process = new MilestoneNonQueryProcesses(_context);

            // Act
            try
            {
                var result = process.Execute(param);
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(Organization), ex.EntityType, "Exception's type was incorrect");
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was incorrect");
                Assert.AreEqual(org.Id, ex.IdValue, "Exception's entity id value was incorrect");
            }
        }

        [TestMethod]
        public void Can_Edit_Milestone()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = true };
            var ms = new MilestoneConfig();
            org.MilestoneConfigurations.Add(ms);
            _context.Users.Add(user);
            _context.SaveChanges();

            var param = new SaveMilestoneParams
            {
                RequestingUserId = user.Id,
                Id = ms.Id,
                Title = "Title",
                Instructions = "Instructions",
                CompletionDisplay = "Completion",
                JobSearchMetrics = new DomainModel.Entities.Metrics.JobSearchMetrics
                {
                    NumApplyTasksCompleted = 1,
                    NumApplyTasksCreated = 2,
                    NumCompaniesCreated = 3,
                    NumContactsCreated = 4,
                    NumInPersonInterviewTasksCreated = 5,
                    NumPhoneInterviewTasksCreated = 6
                }
            };

            IProcess<SaveMilestoneParams, MilestoneIdViewModel> process = new MilestoneNonQueryProcesses(_context);

            // Act
            var result = process.Execute(param);

            // Verify
            var milestone = _context.MilestoneConfigs.SingleOrDefault();

            Assert.IsNotNull(milestone, "No milestone was created");
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(milestone.Id, result.Id, "Result's id value and the milestone's id value did not match");
            Assert.AreEqual(ms, milestone, "Milestone in the database was incorrect");
            Assert.AreEqual(org, milestone.Organization, "Milestone's organization was incorrect");
            Assert.AreEqual(param.Title, milestone.Title, "Milestone's title was incorrect");
            Assert.AreEqual(param.Instructions, milestone.Instructions, "Milestone's instructions were incorrect");
            Assert.AreEqual(param.CompletionDisplay, milestone.CompletionDisplay, "Milestone's completion text was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumApplyTasksCompleted, milestone.JobSearchMetrics.NumApplyTasksCompleted, "Milestones num apply tasks completed was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumApplyTasksCreated, milestone.JobSearchMetrics.NumApplyTasksCreated, "Milestones num apply tasks created was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumCompaniesCreated, milestone.JobSearchMetrics.NumCompaniesCreated, "Milestones num companies was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumContactsCreated, milestone.JobSearchMetrics.NumContactsCreated, "Milestones num contacts was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumInPersonInterviewTasksCreated, milestone.JobSearchMetrics.NumInPersonInterviewTasksCreated, "Milestones num InPersonInterview Tasks completed was incorrect");
            Assert.AreEqual(param.JobSearchMetrics.NumPhoneInterviewTasksCreated, milestone.JobSearchMetrics.NumPhoneInterviewTasksCreated, "Milestones Num InPersonInterview Tasks created was incorrect");
        }

        [TestMethod]
        public void Throws_UserNotAuthorizedForEntityException_When_Editing_Milestone_And_User_Not_Admin_For_Organization()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = false };
            var ms = new MilestoneConfig();
            org.MilestoneConfigurations.Add(ms);
            _context.Users.Add(user);
            _context.SaveChanges();

            var param = new SaveMilestoneParams
            {
                RequestingUserId = user.Id,
                Id = ms.Id
            };

            IProcess<SaveMilestoneParams, MilestoneIdViewModel> process = new MilestoneNonQueryProcesses(_context);

            // Act
            try
            {
                var result = process.Execute(param);
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(Organization), ex.EntityType, "Exception's type was incorrect");
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was incorrect");
                Assert.AreEqual(org.Id, ex.IdValue, "Exception's entity id value was incorrect");
            }
        }
    }
}
