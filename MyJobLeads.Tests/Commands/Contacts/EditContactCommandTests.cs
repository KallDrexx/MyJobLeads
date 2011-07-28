using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;
using Moq;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.Contacts;
using FluentValidation;
using FluentValidation.Results;

namespace MyJobLeads.Tests.Commands.Contacts
{
    [TestClass]
    public class EditContactCommandTests : EFTestBase
    {
        private Contact _contact;
        private User _user;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<UserByIdQuery> _userQuery;
        private Mock<ContactByIdQuery> _contactQuery;
        private Mock<IValidator<Contact>> _validator;

        private void InitializeTestEntities()
        {
            _user = new User();
            _contact = new Contact
            {
                Name = "Starting Name",
                DirectPhone = "444-444-4444",
                MobilePhone = "555-555-5555",
                Extension = "11",
                Email = "starting@email.com",
                Assistant = "Starting Assistant",
                ReferredBy = "Starting Referred",
                Notes = "Starting Notes",

                History = new List<ContactHistory>()
            };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Contacts.Add(_contact);
            _unitOfWork.Commit();

            // Mocks
            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);

            _searchProvider = new Mock<ISearchProvider>();
            _serviceFactory.Setup(x => x.GetService<ISearchProvider>()).Returns(_searchProvider.Object);

            _userQuery = new Mock<UserByIdQuery>(_unitOfWork);
            _userQuery.Setup(x => x.Execute()).Returns(_user);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(_userQuery.Object);

            _contactQuery = new Mock<ContactByIdQuery>(_unitOfWork);
            _contactQuery.Setup(x => x.Execute()).Returns(_contact);
            _serviceFactory.Setup(x => x.GetService<ContactByIdQuery>()).Returns(_contactQuery.Object);

            _validator = new Mock<IValidator<Contact>>();
            _validator.Setup(x => x.Validate(It.IsAny<Contact>())).Returns(new ValidationResult());
            _serviceFactory.Setup(x => x.GetService<IValidator<Contact>>()).Returns(_validator.Object);
        }

        [TestMethod]
        public void Can_Edit_Contact_Details()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
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
            Contact result = new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
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
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Contact_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            _contactQuery.Setup(x => x.Execute()).Returns((Contact)null);
            int id = _contact.Id + 1;

            // Act
            try
            {
                new EditContactCommand(_serviceFactory.Object).WithContactId(id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .RequestedByUserId(_user.Id)
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

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            _userQuery.Setup(x => x.Execute()).Returns((User)null);
            int id = _user.Id + 1;

            // Act
            try
            {
                new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
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
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id)
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
            Assert.AreEqual(MJLConstants.HistoryUpdate, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
        }

        [TestMethod]
        public void Execute_Indexes_Edited_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_serviceFactory.Object).WithContactId(_contact.Id).RequestedByUserId(_user.Id).Execute();

            // Verify
            _searchProvider.Verify(x => x.Index(_contact), Times.Once());
        }

        [TestMethod]
        public void Command_Performs_Contact_Validation()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditContactCommand(_serviceFactory.Object).Execute();

            // Verify
            _validator.Verify(x => x.Validate(It.IsAny<Contact>()), Times.Once());
        }

        [TestMethod]
        public void Command_Throws_ValidationException_On_Contact_Validation_Failure()
        {
            // Setup
            InitializeTestEntities();
            _validator.Setup(x => x.Validate(It.IsAny<Contact>()))
                        .Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("testp", "teste") }));

            // Act
            try
            {
                new EditContactCommand(_serviceFactory.Object).Execute();
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (ValidationException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count(), "Incorrect number of errors returned");
                Assert.AreEqual("testp", ex.Errors.First().PropertyName, "The error's property name was incorrect");
                Assert.AreEqual("teste", ex.Errors.First().ErrorMessage, "The error's message was incorrect");
            }
        }
    }
}
