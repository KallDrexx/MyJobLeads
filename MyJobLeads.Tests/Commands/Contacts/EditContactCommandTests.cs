using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Tests.Commands.Contacts
{
    [TestClass]
    public class EditContactCommandTests : EFTestBase
    {
        private Contact _contact;

        private void InitializeTestEntities()
        {
            _contact = new Contact
            {
                Name = "Starting Name",
                DirectPhone = "444-444-4444",
                MobilePhone = "555-555-5555",
                Extension = "11",
                Email = "starting@email.com",
                Assistant = "Starting Assistant",
                ReferredBy = "Starting Referred",
                Notes = "Starting Notes"
            };
            _unitOfWork.Contacts.Add(_contact);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Edit_Contact_Details()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
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
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Execute_Returns_Edited_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Contact result = new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
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
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Contact_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _contact.Id + 1;

            // Act
            try
            {
                new EditContactCommand(_unitOfWork).WithContactId(id)
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
                Assert.AreEqual(typeof(Contact), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Contact_Name_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
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
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Starting Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_DirectPhone_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("444-444-4444", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_MobilePhone_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("555-555-5555", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_Extension_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("11", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_Assistant_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Starting Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_ReferredBy_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Starting Referred", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_Email_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("starting@email.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The contact's notes was incorrect");
        }

        [TestMethod]
        public void Contact_Notes_Doesnt_Change_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_unitOfWork).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .Execute();

            // Verify
            Contact result = _unitOfWork.Contacts.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No contact existing in the db");
            Assert.AreEqual("Name", result.Name, "The contact's name was incorrect");
            Assert.AreEqual("111-111-1111", result.DirectPhone, "The contact's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", result.MobilePhone, "The contact's mobile phone was incorrect");
            Assert.AreEqual("33", result.Extension, "The contact's extension was incorrect");
            Assert.AreEqual("blah@blah.com", result.Email, "The contact's email was incorrect");
            Assert.AreEqual("Assistant", result.Assistant, "The contact's assistant was incorrect");
            Assert.AreEqual("Referred By", result.ReferredBy, "The contact's referred by was incorrect");
            Assert.AreEqual("Starting Notes", result.Notes, "The contact's notes was incorrect");
        }
    }
}
