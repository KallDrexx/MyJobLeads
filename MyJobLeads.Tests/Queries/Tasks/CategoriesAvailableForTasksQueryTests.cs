using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Queries.Tasks;

namespace MyJobLeads.Tests.Queries.Tasks
{
    [TestClass]
    public class CategoriesAvailableForTasksQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_Categories_Available_For_Tasks()
        {
            // Act
            IList<string> results = new CategoriesAvailableForTasksQuery().Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.IsTrue(results.Count > 0, "Query returned a list with no values");
        }
    }
}
