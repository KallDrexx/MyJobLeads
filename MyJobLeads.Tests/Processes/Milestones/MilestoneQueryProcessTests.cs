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
    }
}
