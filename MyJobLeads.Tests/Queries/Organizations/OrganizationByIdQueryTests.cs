using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Organizations;

namespace MyJobLeads.Tests.Queries.Organizations
{
    [TestClass]
    public class OrganizationByIdQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_Organization()
        {
            // Setup
            Organization org1 = new Organization(), org2 = new Organization(), org3 = new Organization();
            _unitOfWork.Organizations.Add(org1);
            _unitOfWork.Organizations.Add(org2);
            _unitOfWork.Organizations.Add(org3);
            _unitOfWork.Commit();

            // Act
            Organization result = new OrganizationByIdQuery(_serviceFactory.Object).Execute(new OrganizationByIdQueryParams { OrganizationId = org2.Id });

            // Verify
            Assert.AreEqual(org2, result, "Incorrect organization was returned");
        }
    }
}
