using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Commands.Organizations;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.Organizations
{
    [TestClass]
    public class EditOrganizationCommandTests : EFTestBase
    {
        private Organization _org;

        [TestInitialize]
        public void InitializeTestEntities()
        {
            OrganizationEmailDomain domain1 = new OrganizationEmailDomain { Domain = "test@blah.com", IsActive = true };

            _org = new Organization
            {
                Name = "Starting Name",
                RegistrationToken = Guid.NewGuid(),
                IsEmailDomainRestricted = true,
                EmailDomains = new List<OrganizationEmailDomain> { domain1 }
            };

            _unitOfWork.Organizations.Add(_org);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Set_Organization_Name()
        {
            // Act
            new EditOrganizationCommand(_serviceFactory.Object).Execute(new EditOrganizationCommandParams { OrganizationId = _org.Id, Name = "New Name" });
            Organization result = _unitOfWork.Organizations.Fetch().Single();

            // Verify
            Assert.AreEqual("New Name", result.Name, "Organization's name was incorrect");
        }

        [TestMethod]
        public void Can_Enable_Organization_Email_Restriction()
        {
            // Act
            new EditOrganizationCommand(_serviceFactory.Object).Execute(new EditOrganizationCommandParams { OrganizationId = _org.Id, IsEmailDomainRestricted = true });
            Organization result = _unitOfWork.Organizations.Fetch().Single();

            // Verify 
            Assert.IsTrue(result.IsEmailDomainRestricted, "Organization's email domain restriction was not enabled");
        }

        [TestMethod]
        public void Can_Disable_Organization_Email_Restriction()
        {
            // Setup
            _org.IsEmailDomainRestricted = true;
            _unitOfWork.Commit();

            // Act
            new EditOrganizationCommand(_serviceFactory.Object).Execute(new EditOrganizationCommandParams { OrganizationId = _org.Id, IsEmailDomainRestricted = false });
            Organization result = _unitOfWork.Organizations.Fetch().Single();

            // Verify 
            Assert.IsFalse(result.IsEmailDomainRestricted, "Organization's email domain restriction was not disabled");
        }

        [TestMethod]
        public void Can_Replace_Restricted_Email_Domain()
        {
            // Setup
            _org.EmailDomains.Add(new OrganizationEmailDomain { Domain = "blah" });
            _org.EmailDomains.Add(new OrganizationEmailDomain { Domain = "blah2" });
            _unitOfWork.Commit();

            // Act
            new EditOrganizationCommand(_serviceFactory.Object).Execute(new EditOrganizationCommandParams { OrganizationId = _org.Id, EmailDomainRestriction = "test" });
            Organization result = _unitOfWork.Organizations.Fetch().Single();

            // Verify
            Assert.AreEqual(1, result.EmailDomains.Count, "Incorrect number of email domains in the organization");
            Assert.AreEqual("test", result.EmailDomains.First().Domain, "Organization's email domain was incorrect");
        }

        [TestMethod]
        public void Throws_MJLEntityNotFoundException_When_Organization_Not_Found()
        {
            // Setup
            int id = _org.Id + 1;

            // Act
            try
            {
                new EditOrganizationCommand(_serviceFactory.Object).Execute(new EditOrganizationCommandParams { OrganizationId = id });
                Assert.Fail("No exception was thrown");
            }

            // Catch
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Organization), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}