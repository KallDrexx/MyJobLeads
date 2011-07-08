using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Queries.Companies;

namespace MyJobLeads.Tests.Queries.Companies
{
    [TestClass]
    public class LeadStatusesAvailableForCompaniesQueryTests
    {
        [TestMethod]
        public void Can_Retrieve_Lead_Statuses_Available_For_Companies()
        {
            // Act
            IList<string> results = new LeadStatusesAvailableForCompaniesQuery().Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.IsTrue(results.Count > 0, "Query returned a list with no values");
        }
    }
}
