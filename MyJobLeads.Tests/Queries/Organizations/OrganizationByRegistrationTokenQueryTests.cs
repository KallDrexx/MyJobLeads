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
    public class OrganizationByRegistrationTokenQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_Organization_By_Registration_Token()
        {
            // Setup
            Organization org1 = new Organization { RegistrationToken = Guid.NewGuid() };
            Organization org2 = new Organization { RegistrationToken = Guid.NewGuid() };
            Organization org3 = new Organization { RegistrationToken = Guid.NewGuid() };

            _unitOfWork.Organizations.Add(org1);
            _unitOfWork.Organizations.Add(org2);
            _unitOfWork.Organizations.Add(org3);
            _unitOfWork.Commit();

            // Act
            Organization result = new OrganizationByRegistrationTokenQuery(_serviceFactory.Object).Execute(new OrganizationByRegistrationTokenQueryParams
            {
                RegistrationToken = org2.RegistrationToken
            });
            
            // Verify
            Assert.AreEqual(org2, result, "Query returned an incorrect registration token");
        }

        [TestMethod]
        public void Query_Returns_Null_When_No_Org_Has_Matching_Registration_Token()
        {
            // Setup
            Organization org1 = new Organization { RegistrationToken = Guid.NewGuid() };
            _unitOfWork.Organizations.Add(org1);
            _unitOfWork.Commit();

            // Act
            Organization result = new OrganizationByRegistrationTokenQuery(_serviceFactory.Object).Execute(new OrganizationByRegistrationTokenQueryParams
            {
                RegistrationToken = Guid.NewGuid()
            });

            // Verify
            Assert.IsNull(result, "Query did not return a null object");
        }
    }
}
