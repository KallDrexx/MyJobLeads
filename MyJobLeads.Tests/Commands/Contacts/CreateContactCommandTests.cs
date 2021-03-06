﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;
using Moq;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Commands.JobSearches;
using FluentValidation;
using FluentValidation.Results;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.Tests.Queries;

namespace MyJobLeads.Tests.Commands.Contacts
{
    [TestClass]
    public class CreateContactCommandTests : EFTestBase
    {
        private JobSearch _jobSearch;
        private Company _company;
        private User _user;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<UpdateJobSearchMetricsCommand> _updateMetricsCmd;
        private Mock<IValidator<Contact>> _validator;
        private Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>> _companyAuthMock;

        private void InitializeTestEntities()
        {
            _searchProvider = new Mock<ISearchProvider>();
            _jobSearch = new JobSearch();
            _company = new Company { JobSearch = _jobSearch };
            _user = new User();

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Companies.Add(_company);
            _unitOfWork.Commit();

            // Mocks
            _companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            _companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);

            _searchProvider = new Mock<ISearchProvider>();
            _serviceFactory.Setup(x => x.GetService<ISearchProvider>()).Returns(_searchProvider.Object);

            Mock<UserByIdQuery> userQuery = new Mock<UserByIdQuery>(_unitOfWork);
            userQuery.Setup(x => x.Execute()).Returns(_user);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(userQuery.Object);

            Mock<CompanyByIdQuery> companyQuery = QueryTestUtils.GenerateCompanyByIdQueryMock();
            companyQuery.Setup(x => x.Execute()).Returns(_company);
            _serviceFactory.Setup(x => x.GetService<CompanyByIdQuery>()).Returns(companyQuery.Object);

            _updateMetricsCmd = new Mock<UpdateJobSearchMetricsCommand>(_serviceFactory.Object);
            _serviceFactory.Setup(x => x.GetService<UpdateJobSearchMetricsCommand>()).Returns(_updateMetricsCmd.Object);

            _validator = new Mock<IValidator<Contact>>();
            _validator.Setup(x => x.Validate(It.IsAny<Contact>())).Returns(new ValidationResult());
            _serviceFactory.Setup(x => x.GetService<IValidator<Contact>>()).Returns(_validator.Object);
        }

        [TestMethod]
        public void Can_Create_New_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateContactCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .SetTitle("Contact Title")
                                                 .SetJigsawId(1234)
                                                 .SetHasJigsawAccess(true)
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
            Assert.AreEqual(1234, result.JigsawId, "Jigsaw Id was incorrect");
            Assert.IsTrue(result.HasJigsawAccess, "Has jigsaw access waas incorrect");
            Assert.AreEqual(_company, result.Company, "The contact is associated with the incorrect company");
            Assert.AreEqual("Contact Title", result.Title, "The contact's title was incorrect");
        }

        [TestMethod]
        public void Execute_Returns_Created_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Contact result = new CreateContactCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
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

            Mock<CompanyByIdQuery> query = QueryTestUtils.GenerateCompanyByIdQueryMock();
            query.Setup(x => x.Execute()).Returns((Company)null);
            _serviceFactory.Setup(x => x.GetService<CompanyByIdQuery>()).Returns(query.Object);

            // Act
            try
            {
                new CreateContactCommand(_serviceFactory.Object).WithCompanyId(id)
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
            Contact result = new CreateContactCommand(_serviceFactory.Object).WithCompanyId(_company.Id).RequestedByUserId(_user.Id).Execute();

            // Verify
            Assert.IsNotNull(result.Tasks, "Contact's task list was not initialized");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 1;

            Mock<UserByIdQuery> query = new Mock<UserByIdQuery>(_unitOfWork);
            query.Setup(x => x.Execute()).Returns((User)null);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(query.Object);

            // Act
            try
            {
                new CreateContactCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
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
            new CreateContactCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetDirectPhone("111-111-1111")
                                                 .SetMobilePhone("222-222-2222")
                                                 .SetExtension("33")
                                                 .SetEmail("blah@blah.com")
                                                 .SetAssistant("Assistant")
                                                 .SetReferredBy("Referred By")
                                                 .SetNotes("Notes")
                                                 .SetTitle("Title")
                                                 .SetJigsawId(1234)
                                                 .SetHasJigsawAccess(true)
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
            Assert.AreEqual("Title", history.Title, "The history record's title was incorrect");
            Assert.AreEqual(1234, history.JigsawId, "Jigsaw Id was incorrect");
            Assert.IsTrue(history.HasJigsawAccess, "Has Jigsaw Access was incorrect");
            Assert.AreEqual(_user, history.AuthoringUser, "The history record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryInsert, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
        }

        [TestMethod]
        public void Execute_Indexes_Created_Contact()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Contact contact = new CreateContactCommand(_serviceFactory.Object)
                                    .WithCompanyId(_company.Id)
                                    .RequestedByUserId(_user.Id)
                                    .Execute();

            // Verify
            _searchProvider.Verify(x => x.Index(contact), Times.Once());
        }

        [TestMethod]
        public void Execute_Updates_JobSearch_Metrics()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateContactCommand(_serviceFactory.Object).Execute();

            // Verify
            _updateMetricsCmd.Verify(x => x.Execute(It.Is<UpdateJobSearchMetricsCmdParams>(y => y.JobSearchId == _jobSearch.Id)));
        }

        [TestMethod]
        public void Command_Performs_Contact_Validation()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateContactCommand(_serviceFactory.Object).Execute();

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
                new CreateContactCommand(_serviceFactory.Object).Execute();
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