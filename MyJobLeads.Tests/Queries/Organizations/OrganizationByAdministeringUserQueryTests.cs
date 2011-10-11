using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Organizations;
using MyJobLeads.DomainModel.ViewModels.Organizations;

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
            OrganizationDashboardViewModel result =
                new OrganizationByAdministeringUserQuery(_serviceFactory.Object).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(org1, result.Organization, "Query returned an incorrect organization");
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
            OrganizationDashboardViewModel result =
                new OrganizationByAdministeringUserQuery(_serviceFactory.Object).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.IsNull(result, "Query did not return a null organization");
        }

        [TestMethod]
        public void Can_Get_All_Non_Member_Official_Documents()
        {
            // Setup
            Organization org = new Organization();
            User user = new User { Organization = org, IsOrganizationAdmin = true };
            _unitOfWork.Users.Add(user);

            _unitOfWork.OfficialDocuments.Add(new OfficialDocument { Name = "Doc 1", MeantForMembers = false });
            _unitOfWork.OfficialDocuments.Add(new OfficialDocument { Name = "Doc 2", MeantForMembers = true });
            _unitOfWork.OfficialDocuments.Add(new OfficialDocument { Name = "Doc 3", MeantForMembers = false });

            _unitOfWork.Commit();

            // Act
            OrganizationDashboardViewModel result =
                new OrganizationByAdministeringUserQuery(_serviceFactory.Object).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user.Id });

            // Verify
            Assert.AreEqual(2, result.NonMemberOfficialDocuments.Count, "Incorrect number of official non-member documents retrieved");
            Assert.IsTrue(result.NonMemberOfficialDocuments.Any(x => x.Name == "Doc 1"), "Document list did not contain doc 1");
            Assert.IsTrue(result.NonMemberOfficialDocuments.Any(x => x.Name == "Doc 3"), "Document list did not contain doc 3");
        }
    }
}
