using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Processes.Organizations;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.Organizations
{
    [TestClass]
    public class OrganizationDocumentProcessTests : EFTestBase
    {
        [TestMethod]
        public void Can_Set_Document_As_Visible_To_Members()
        {
            // Setup
            IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel> process = new OrganizationDocumentProcesses(_context);

            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = true };
            var doc1 = new OfficialDocument { MeantForMembers = true };
            var doc2 = new OfficialDocument { MeantForMembers = true };

            _context.Organizations.Add(org);
            _context.Users.Add(user);
            _context.OfficialDocuments.Add(doc1);
            _context.OfficialDocuments.Add(doc2);
            _context.SaveChanges();

            // Act
            var result = process.Execute(new OrgMemberDocVisibilityByOrgAdminParams
            {
                OfficialDocumentId = doc2.Id,
                RequestingUserId = user.Id,
                ShowToMembers = true
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.WasSuccessful, "Process did not return a successful result");
            Assert.AreEqual(1, org.MemberOfficialDocuments.Count, "Organization had an incorrect number of member documents");
            Assert.IsTrue(org.MemberOfficialDocuments.Contains(doc2), "Organization does not have the document as visible to members");
        }

        [TestMethod]
        public void Can_Set_Document_As_Not_Visible_To_Members()
        {
            // Setup
            IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel> process = new OrganizationDocumentProcesses(_context);

            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = true };
            var doc = new OfficialDocument { MeantForMembers = true };
            org.MemberOfficialDocuments.Add(doc);

            _context.Organizations.Add(org);
            _context.Users.Add(user);
            _context.OfficialDocuments.Add(doc);
            _context.SaveChanges();

            // Act
            var result = process.Execute(new OrgMemberDocVisibilityByOrgAdminParams
            {
                OfficialDocumentId = doc.Id,
                RequestingUserId = user.Id,
                ShowToMembers = false
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.WasSuccessful, "Process did not return a successful result");
            Assert.AreEqual(0, org.MemberOfficialDocuments.Count, "Organization had an incorrect number of member documents");
        }

        [TestMethod]
        public void Throws_Exception_When_User_Not_Organization_Admin()
        {
            // Setup
            IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel> process = new OrganizationDocumentProcesses(_context);

            var org = new Organization();
            var user = new User { Organization = org, IsOrganizationAdmin = false };
            var doc = new OfficialDocument { MeantForMembers = true };
            org.MemberOfficialDocuments.Add(doc);

            _context.Users.Add(user);
            _context.OfficialDocuments.Add(doc);
            _context.SaveChanges();

            // Act
            try
            {
                process.Execute(new OrgMemberDocVisibilityByOrgAdminParams
                {
                    OfficialDocumentId = doc.Id,
                    RequestingUserId = user.Id,
                    ShowToMembers = false
                });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was not correct");
                Assert.AreEqual(user.Organization.Id, ex.IdValue, "Exception's entity id value was not correct");
                Assert.AreEqual(typeof(Organization), ex.EntityType, "Exception's entity type value was not correct");
            }
        }
    }
}
