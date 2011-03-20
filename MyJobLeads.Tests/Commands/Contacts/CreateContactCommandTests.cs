using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.Contacts
{
    [TestClass]
    public class CreateContactCommandTests : EFTestBase
    {
        private Company _company;

        private void InitializeTestEntities()
        {
            _company = new Company();
            _unitOfWork.Companies.Add(_company);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Create_New_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateContactCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact was created");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
            Assert.AreEqual(_company, result.Company, "The contact is associated with the incorrect company");
        }

        [TestMethod]
        public void Execute_Returns_Created_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Contact result = new CreateContactCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                                 .SetName("Name")
                                                                 .SetDirectPhone("111-111-1111")
                                                                 .SetMobilePhone("222-222-2222")
                                                                 .SetExtension("33")
                                                                 .SetEmail("blah@blah.com")
                                                                 .SetAssistant("Assistant")
                                                                 .SetReferredBy("Referred By")
                                                                 .SetNotes("Notes")
                                                                 .Execute();

            // Verify
            Assert.IsNotNull(result, "No contact was created");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
            Assert.AreEqual(_company, result.Company, "The contact is associated with the incorrect company");
        }
       
        [TestMethod]
        public void Execute_Throws_Exception_When_Company_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _company.Id + 1;

            // Act
            try
            {
                new CreateContactCommand(_unitOfWork).WithCompanyId(id)
                                                    .SetName("Name")
                                                    .SetDirectPhone("111-111-1111")
                                                    .SetMobilePhone("222-222-2222")
                                                    .SetExtension("33")
                                                    .SetEmail("blah@blah.com")
                                                    .SetAssistant("Assistant")
                                                    .SetReferredBy("Referred By")
                                                    .SetNotes("Notes")
                                                    .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Company), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}
