using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Queries.MilestoneConfigs;
using MyJobLeads.DomainModel.Data;

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

            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);

            // Act
            MilestoneConfig result = new StartingMilestoneQuery(_serviceFactory.Object).Execute();

            // Verify
            Assert.AreEqual(config2, result, "The returned milestone configuration was incorrect");
        }
    }
}
