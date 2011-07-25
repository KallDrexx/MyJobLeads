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
    public class OrganizationByAdministeringUserQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Get_Organization_By_Administrating_User()
        {
            // Setup
            Organization org1 = new Organization();
            Organization org2 = new Organization();

            User user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            User user2 = new User { Organization = org2 };

            _unitOfWork.Users.Add(user1);
            _unitOfWork.Users.Add(user2);
            _unitOfWork.Commit();

            // Act
            Organization result =
                new OrganizationByAdministeringUserQuery(_serviceFactory.Object).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(org1, result, "Query returned an incorrect organization");
        }

        [TestMethod]
        public void Query_Returns_Null_When_User_Isnt_Org_Admin()
        {
            // Setup
            Organization org1 = new Organization();
            User user1 = new User { Organization = org1, IsOrganizationAdmin = false };
            _unitOfWork.Users.Add(user1);
            _unitOfWork.Commit();

            // Act
            Organization result =
                new OrganizationByAdministeringUserQuery(_serviceFactory.Object).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.IsNull(result, "Query did not return a null organization");
        }
    }
}
