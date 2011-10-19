using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.ViewModels.Milestones;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Milestones;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.Metrics;
using MyJobLeads.DomainModel.ProcessParams.Milestones;

namespace MyJobLeads.Tests.Processes.Milestones
{
    [TestClass]
    public class MilestoneQueryProcessTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_Milestones_For_Organization()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user = new User { Organization = org1, IsOrganizationAdmin = true };
            var m1 = new MilestoneConfig();
            var m2 = new MilestoneConfig();
            var m3 = new MilestoneConfig();
            org1.MilestoneConfigurations.Add(m1);
            org2.MilestoneConfigurations.Add(m2);
            org1.MilestoneConfigurations.Add(m3);

            _context.Organizations.Add(org1);
            _context.Organizations.Add(org2);
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel> process = new MilestoneQueryProcesses(_context);

            // Act
            var result = process.Execute(new ByOrganizationIdParams { RequestingUserId = user.Id, OrganizationId = org1.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsNotNull(result.Milestones, "Process returned a null list of milestones");
            Assert.AreEqual(2, result.Milestones.Count, "Process returned a incorrect number of milestones");
            Assert.IsTrue(result.Milestones.Any(x => x.Id == m1.Id), "Results did not have the first milestone");
            Assert.IsTrue(result.Milestones.Any(x => x.Id == m3.Id), "Results did not have the third milestone");
        }

        [TestMethod]
        public void Throws_NotAuthorizedException_When_User_Not_Org_Admin()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = false };
            var m1 = new MilestoneConfig();
            org.MilestoneConfigurations.Add(m1);

            _context.Users.Add(user);
            _context.Organizations.Add(org);
            _context.SaveChanges();

            IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel> process = new MilestoneQueryProcesses(_context);

            // Act
            try
            {
                var result = process.Execute(new ByOrganizationIdParams { RequestingUserId = user.Id, OrganizationId = org.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(org.Id, ex.IdValue, "Exception's id value was incorrect");
                Assert.AreEqual(typeof(Organization), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was incorrect");
            }
        }

        [TestMethod]
        public void Can_Retrieve_Milestone_Data()
        {
            // Setup
            var user = new User { IsOrganizationAdmin = true };
            var org = new Organization();
            org.Members.Add(user);
            var ms3 = new MilestoneConfig { Organization = org };
            
            var ms2 = new MilestoneConfig 
            { 
                Organization = org, 
                Title = "title",
                Instructions = "instructions",
                NextMilestone = ms3,
                JobSearchMetrics = new JobSearchMetrics
                {
                    NumApplyTasksCompleted = 1,
                    NumApplyTasksCreated = 2,
                    NumCompaniesCreated = 3,
                    NumContactsCreated = 4,
                    NumInPersonInterviewTasksCreated = 5,
                    NumPhoneInterviewTasksCreated = 6
                }
            };

            var ms1 = new MilestoneConfig { Organization = org, NextMilestone = ms2, IsStartingMilestone = true, Title = "previous title" };

            _context.MilestoneConfigs.Add(ms1);
            _context.SaveChanges();

            IProcess<MilestoneIdParams, MilestoneDisplayViewModel> process = new MilestoneQueryProcesses(_context);

            // Act
            var result = process.Execute(new MilestoneIdParams { MilestoneId = ms2.Id, RequestingUserId = user.Id });

            // Setup
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(ms2.Id, result.Id, "Id value was incorrect");
            Assert.AreEqual(ms2.Title, result.Title, "Title was incorrect");
            Assert.AreEqual(ms2.Instructions, result.Instructions, "Instructions value was incorrect");
            Assert.AreEqual(ms2.CompletionDisplay, result.CompletionDisplay, "Completion display was incorrect");
            Assert.AreEqual(ms2.JobSearchMetrics, result.JobSearchMetrics, "Job Search metrics were incorrect");
            Assert.AreEqual(ms1.Id, result.PreviousMilestoneId, "Previous milestone id was incorrect");
            Assert.AreEqual(ms1.Title, result.PreviousMilestoneName, "Previous milestone title was icorrect");
        }

        [TestMethod]
        public void Throws_EntityNotFoundException_If_Milestone_Doesnt_Exist()
        {
            // Setup
            var user = new User();
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<MilestoneIdParams, MilestoneDisplayViewModel> process = new MilestoneQueryProcesses(_context);
            int id = 5;

            // Act
            try
            {
                process.Execute(new MilestoneIdParams { MilestoneId = id, RequestingUserId = user.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(MilestoneConfig), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
            }
        }

        [TestMethod]
        public void Throws_EntityNotFoundException_If_User_Doesnt_Exist()
        {
            // Setup
            var ms = new MilestoneConfig();
            _context.MilestoneConfigs.Add(ms);
            _context.SaveChanges();

            IProcess<MilestoneIdParams, MilestoneDisplayViewModel> process = new MilestoneQueryProcesses(_context);
            int id = 5;

            // Act
            try
            {
                process.Execute(new MilestoneIdParams { MilestoneId = ms.Id, RequestingUserId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
            }
        }

        [TestMethod]
        public void Throws_UserNotAuthorizedForEntityException_When_User_Not_Organization_Admin()
        {
            // Setup
            var user = new User { IsOrganizationAdmin = false };
            var org = new Organization();
            var ms = new MilestoneConfig { Organization = org, IsStartingMilestone = true };
            org.Members.Add(user);

            _context.MilestoneConfigs.Add(ms);
            _context.SaveChanges();

            IProcess<MilestoneIdParams, MilestoneDisplayViewModel> process = new MilestoneQueryProcesses(_context);
            
            // Act
            try
            {
                process.Execute(new MilestoneIdParams { MilestoneId = ms.Id, RequestingUserId = user.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(MilestoneConfig), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was incorrect");
                Assert.AreEqual(ms.Id, ex.IdValue, "Exception's id value was incorrect");
            }
        }

        [TestMethod]
        public void Throws_UserNotAuthorizedForEntityException_When_User_Not_Part_Of_Same_Organization_As_Milestone()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user = new User { IsOrganizationAdmin = true, Organization = org1 };
            var ms = new MilestoneConfig { Organization = org2, IsStartingMilestone = true };

            _context.MilestoneConfigs.Add(ms);
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<MilestoneIdParams, MilestoneDisplayViewModel> process = new MilestoneQueryProcesses(_context);

            // Act
            try
            {
                process.Execute(new MilestoneIdParams { MilestoneId = ms.Id, RequestingUserId = user.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(MilestoneConfig), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was incorrect");
                Assert.AreEqual(ms.Id, ex.IdValue, "Exception's id value was incorrect");
            }
        }
    }
}
