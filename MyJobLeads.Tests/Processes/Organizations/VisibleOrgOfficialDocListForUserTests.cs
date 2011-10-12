using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Users;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Organizations;

namespace MyJobLeads.Tests.Processes.Organizations
{
    [TestClass]
    public class VisibleOrgOfficialDocListForUserTests : EFTestBase
    {
        [TestMethod]
        public void Can_Get_List_Of_Documents_Visible_To_User()
        {
            // Setup
            var doc1 = new OfficialDocument { MeantForMembers = true };
            var doc2 = new OfficialDocument { MeantForMembers = true };
            var doc3 = new OfficialDocument { MeantForMembers = true };
            var org = new Organization();
            var user = new User { Organization = org };
            org.MemberOfficialDocuments.Add(doc1);
            org.MemberOfficialDocuments.Add(doc3);
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<ByUserIdParams, VisibleOrgOfficialDocListForUserViewModel> process =
                new OrganizationDocumentProcesses(_context);

            // Act
            VisibleOrgOfficialDocListForUserViewModel result = process.Execute(new ByUserIdParams { UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process result is null");
            Assert.AreEqual(2, result.UserVisibleDocuments.Count, "Document list contained an incorrect number of elements");
            Assert.IsTrue(result.UserVisibleDocuments.Contains(doc1), "Document list did not contain doc1");
            Assert.IsTrue(result.UserVisibleDocuments.Contains(doc3), "Document list did not contain doc3");
        }

        [TestMethod]
        public void Returns_Empty_List_When_User_Id_Not_Found()
        {
            // Setup
            IProcess<ByUserIdParams, VisibleOrgOfficialDocListForUserViewModel> process =
                new OrganizationDocumentProcesses(_context);

            // Act
            VisibleOrgOfficialDocListForUserViewModel result = process.Execute(new ByUserIdParams { UserId = 0 });

            // Verify
            Assert.IsNotNull(result, "Process result is null");
            Assert.AreEqual(0, result.UserVisibleDocuments.Count, "Document list contained an incorrect number of elements");
        }

        [TestMethod]
        public void Returns_Empty_List_When_User_Not_In_Organization()
        {
            // Setup
            var user = new User();
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<ByUserIdParams, VisibleOrgOfficialDocListForUserViewModel> process =
                new OrganizationDocumentProcesses(_context);

            // Act
            VisibleOrgOfficialDocListForUserViewModel result = process.Execute(new ByUserIdParams { UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process result is null");
            Assert.AreEqual(0, result.UserVisibleDocuments.Count, "Document list contained an incorrect number of elements");
        }
    }
}
