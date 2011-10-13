using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Organizations;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel;

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
                new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

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
                new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

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
                new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user.Id });

            // Verify
            Assert.AreEqual(2, result.NonMemberOfficialDocuments.Count, "Incorrect number of official non-member documents retrieved");
            Assert.IsTrue(result.NonMemberOfficialDocuments.Any(x => x.Name == "Doc 1"), "Document list did not contain doc 1");
            Assert.IsTrue(result.NonMemberOfficialDocuments.Any(x => x.Name == "Doc 3"), "Document list did not contain doc 3");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var user3 = new User { Organization = org1 };

            _context.Users.Add(user1);
            _context.Users.Add(user2);
            _context.Users.Add(user3);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumMembers, "Number of members was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Member_Companies()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var company3 = new Company { JobSearch = js1 };

            _context.Companies.Add(company1);
            _context.Companies.Add(company2);
            _context.Companies.Add(company3);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumCompanies, "Number of companies was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Member_Contacts()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var contact1 = new Contact { Company = company1 };
            var contact2 = new Contact { Company = company2 };
            var contact3 = new Contact { Company = company1 };

            _context.Contacts.Add(contact1);
            _context.Contacts.Add(contact2);
            _context.Contacts.Add(contact3);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumContacts, "Number of contacts was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Open_Phone_Interview_Tasks_For_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var task1 = new Task { Company = company1, Category = MJLConstants.PhoneInterviewTaskCategory };
            var task2 = new Task { Company = company2, Category = MJLConstants.PhoneInterviewTaskCategory };
            var task3 = new Task { Company = company1, Category = MJLConstants.PhoneInterviewTaskCategory, CompletionDate = DateTime.Now };
            var task4 = new Task { Company = company1, Category = MJLConstants.PhoneInterviewTaskCategory };

            _context.Tasks.Add(task1);
            _context.Tasks.Add(task2);
            _context.Tasks.Add(task3);
            _context.Tasks.Add(task4);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumOpenPhoneTasks, "Number of open phone tasks was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Closed_Phone_Interview_Tasks_For_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var task1 = new Task { Company = company1, Category = MJLConstants.PhoneInterviewTaskCategory };
            var task2 = new Task { Company = company2, Category = MJLConstants.PhoneInterviewTaskCategory };
            var task3 = new Task { Company = company1, Category = MJLConstants.PhoneInterviewTaskCategory, CompletionDate = DateTime.Now };
            var task4 = new Task { Company = company1, Category = MJLConstants.PhoneInterviewTaskCategory };

            _context.Tasks.Add(task1);
            _context.Tasks.Add(task2);
            _context.Tasks.Add(task3);
            _context.Tasks.Add(task4);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(1, result.NumClosedPhoneTasks, "Number of closed phone tasks was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Open_InPerson_Interview_Tasks_For_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var task1 = new Task { Company = company1, Category = MJLConstants.InPersonInterviewTaskCategory };
            var task2 = new Task { Company = company2, Category = MJLConstants.InPersonInterviewTaskCategory };
            var task3 = new Task { Company = company1, Category = MJLConstants.InPersonInterviewTaskCategory, CompletionDate = DateTime.Now };
            var task4 = new Task { Company = company1, Category = MJLConstants.InPersonInterviewTaskCategory };

            _context.Tasks.Add(task1);
            _context.Tasks.Add(task2);
            _context.Tasks.Add(task3);
            _context.Tasks.Add(task4);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumOpenInterviewTasks, "Number of open in person interview tasks was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Closed_InPerson_Interview_Tasks_For_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var task1 = new Task { Company = company1, Category = MJLConstants.InPersonInterviewTaskCategory };
            var task2 = new Task { Company = company2, Category = MJLConstants.InPersonInterviewTaskCategory };
            var task3 = new Task { Company = company1, Category = MJLConstants.InPersonInterviewTaskCategory, CompletionDate = DateTime.Now };
            var task4 = new Task { Company = company1, Category = MJLConstants.InPersonInterviewTaskCategory };

            _context.Tasks.Add(task1);
            _context.Tasks.Add(task2);
            _context.Tasks.Add(task3);
            _context.Tasks.Add(task4);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(1, result.NumClosedInterviewTasks, "Number of closed in person interview tasks was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Applied_Positions_For_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var pos1 = new Position { Company = company1, HasApplied = true };
            var pos2 = new Position { Company = company2, HasApplied = true };
            var pos3 = new Position { Company = company1, HasApplied = false };
            var pos4 = new Position { Company = company1, HasApplied = true };

            _context.Positions.Add(pos1);
            _context.Positions.Add(pos2);
            _context.Positions.Add(pos3);
            _context.Positions.Add(pos4);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumAppliedPositions, "Number of applied positions was incorrect");
        }

        [TestMethod]
        public void Can_Get_Number_Of_Unapplied_Positions_For_Members()
        {
            // Setup
            var org1 = new Organization();
            var org2 = new Organization();
            var user1 = new User { Organization = org1, IsOrganizationAdmin = true };
            var user2 = new User { Organization = org2 };
            var js1 = new JobSearch { User = user1 };
            var js2 = new JobSearch { User = user2 };
            var company1 = new Company { JobSearch = js1 };
            var company2 = new Company { JobSearch = js2 };
            var pos1 = new Position { Company = company1, HasApplied = false };
            var pos2 = new Position { Company = company2, HasApplied = false };
            var pos3 = new Position { Company = company1, HasApplied = true };
            var pos4 = new Position { Company = company1, HasApplied = false };

            _context.Positions.Add(pos1);
            _context.Positions.Add(pos2);
            _context.Positions.Add(pos3);
            _context.Positions.Add(pos4);
            _context.SaveChanges();

            // Act
            var result = new OrganizationByAdministeringUserQuery(_context).Execute(new OrganizationByAdministeringUserQueryParams { AdministeringUserId = user1.Id });

            // Verify
            Assert.AreEqual(2, result.NumNotAppliedPositions, "Number of not-applied positions was incorrect");
        }
    }
}
