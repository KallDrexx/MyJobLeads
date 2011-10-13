using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Queries.MilestoneConfigs;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Tests.Queries.MilestoneConfigs
{
    [TestClass]
    public class StartingMilestoneQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_First_Seen_Starting_Milestone()
        {
            // Setup
            MilestoneConfig config1 = new MilestoneConfig { IsStartingMilestone = false };
            MilestoneConfig config2 = new MilestoneConfig { IsStartingMilestone = true };
            MilestoneConfig config3 = new MilestoneConfig { IsStartingMilestone = false };
            _unitOfWork.MilestoneConfigs.Add(config1);
            _unitOfWork.MilestoneConfigs.Add(config2);
            _unitOfWork.MilestoneConfigs.Add(config3);
            _unitOfWork.Commit();

            // Act
            MilestoneConfig result = new StartingMilestoneQuery(_context).Execute(new StartingMilestoneQueryParams());

            // Verify
            Assert.AreEqual(config2, result, "The returned milestone configuration was incorrect");
        }

        [TestMethod]
        public void Does_Not_Return_Starting_Milestone_For_Organization_If_None_Requested()
        {
            // Setup
            var org = new Organization();
            var config1 = new MilestoneConfig { IsStartingMilestone = true, Organization = org };
            var config2 = new MilestoneConfig { IsStartingMilestone = true };
            var config3 = new MilestoneConfig { IsStartingMilestone = true, Organization = org };

            _context.MilestoneConfigs.Add(config1);
            _context.MilestoneConfigs.Add(config2);
            _context.MilestoneConfigs.Add(config3);
            _context.SaveChanges();

            // Act
            MilestoneConfig result = new StartingMilestoneQuery(_context).Execute(new StartingMilestoneQueryParams());

            // Verify
            Assert.AreEqual(config2, result, "The returned milestone was incorrect");
        }

        [TestMethod]
        public void Returns_Organizations_Starting_Milestone_If_OrganizationId_Is_Given()
        {
            // Setup
            var org = new Organization();
            var config1 = new MilestoneConfig { IsStartingMilestone = true };
            var config2 = new MilestoneConfig { IsStartingMilestone = true, Organization = org };
            var config3 = new MilestoneConfig { IsStartingMilestone = true };

            _context.MilestoneConfigs.Add(config1);
            _context.MilestoneConfigs.Add(config2);
            _context.MilestoneConfigs.Add(config3);
            _context.SaveChanges();

            // Act
            MilestoneConfig result = new StartingMilestoneQuery(_context).Execute(new StartingMilestoneQueryParams { OrganizationId = org.Id });

            // Verify
            Assert.AreEqual(config2, result, "The returned milestone was incorrect");
        }
    }
}
