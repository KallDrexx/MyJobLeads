using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;

namespace MyJobLeads.Tests.Commands.Contacts
{
    [TestClass]
    public class CreateContactCommandTests : EFTestBase
    {
        private Company _company;
        private User _user;

        private void InitializeTestEntities()
        {
            _company = new Company();
            _user = new User();

            _unitOfWork.Users.Add(_user);
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
                                                 .RequestedByUserId(_user.Id)
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
                                                                 .RequestedByUserId(_user.Id)
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
                                                    .RequestedByUserId(_user.Id)
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

        [TestMethod]
        public void Execute_Initializes_Task_List()
        {
            // setup
            InitializeTestEntities();

            // Act
            Contact result = new CreateContactCommand(_unitOfWork).WithCompanyId(_company.Id).RequestedByUserId(_user.Id).Execute();

            // Verify
            Assert.IsNotNull(result.Tasks, "Contact's task list was not initialized");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 1;

            // Act
            try
            {
                new CreateContactCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                    .SetName("Name")
                                                    .SetDirectPhone("111-111-1111")
                                                    .SetMobilePhone("222-222-2222")
                                                    .SetExtension("33")
                                                    .SetEmail("blah@blah.com")
                                                    .SetAssistant("Assistant")
                                                    .SetReferredBy("Referred By")
                                                    .SetNotes("Notes")
                                                    .RequestedByUserId(id)
                                                    .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Creates_History_Record()
        {
            // Setup
            InitializeTestEntities();

            // Act
            DateTime start = DateTime.Now;
            new CreateContactCommand(_unitOfWork).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();
            DateTime end = DateTime.Now;

            // Verify
            Contact contact = _unitOfWork.Contacts.Fetch().Single();
            ContactHistory history = contact.History.Single();

            Assert.AreEqual("Name", history.Name, "The history record's name was incorrect");
            Assert.AreEqual("111-111-1111", history.DirectPhone, "The history record's direct phone was incorrect");
            Assert.AreEqual("222-222-2222", history.MobilePhone, "The history record's mobile phone was incorrect");
            Assert.AreEqual("33", history.Extension, "The history record's extension was incorrect");
            Assert.AreEqual("blah@blah.com", history.Email, "The history record's email was incorrect");
            Assert.AreEqual("Assistant", history.Assistant, "The history record's assistant was incorrect");
            Assert.AreEqual("Referred By", history.ReferredBy, "The history record's referred by was incorrect");
            Assert.AreEqual("Notes", history.Notes, "The history record's notes was incorrect");
            Assert.AreEqual(_user, history.AuthoringUser, "The history record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryInsert, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
        }
    }
}
