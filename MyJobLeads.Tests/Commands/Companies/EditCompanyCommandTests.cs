using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel;
using MyJobLeads.DomainModel.Providers.Search;
using Moq;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.Companies;

namespace MyJobLeads.Tests.Commands.Companies
{
    [TestClass]
    public class EditCompanyCommandTests : EFTestBase
    {
        private Company _company;
        private User _user;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<IServiceFactory> _serviceFactory;

        private void InitializeTestEntities()
        {
            _user = new User();
            _company = new Company
            {
                Name = "Starting Name",
                Phone = "333-444-5555",
                City = "Starting City",
                State = "Starting State",
                Zip = "00000",
                MetroArea = "Starting Metro",
                Industry = "Starting Industry",
                Notes = "Starting Notes", 

                History = new List<CompanyHistory>()
            };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Companies.Add(_company);
            _unitOfWork.Commit();

            // Setup mocks
            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);

            _searchProvider = new Mock<ISearchProvider>();
            _serviceFactory.Setup(x => x.GetService<ISearchProvider>()).Returns(_searchProvider.Object);

            Mock<UserByIdQuery> userQuery = new Mock<UserByIdQuery>(_unitOfWork);
            userQuery.Setup(x => x.Execute()).Returns(_user);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(userQuery.Object);

            Mock<CompanyByIdQuery> companyQuery = new Mock<CompanyByIdQuery>(_unitOfWork);
            companyQuery.Setup(x => x.Execute()).Returns(_company);
            _serviceFactory.Setup(x => x.GetService<CompanyByIdQuery>()).Returns(companyQuery.Object);
        }

        [TestMethod]
        public void Can_Edit_New_Company_For_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Execute_Returns_Created_Company()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company result = new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                                 .SetName("Name")
                                                                 .SetPhone("555-555-5555")
                                                                 .SetCity("City")
                                                                 .SetState("State")
                                                                 .SetZip("01234")
                                                                 .SetMetroArea("Metro")
                                                                 .SetIndustry("Industry")
                                                                 .SetNotes("Notes")
                                                                 .RequestedByUserId(_user.Id)
                                                                 .Execute();

            // Verify
            Assert.IsNotNull(result, "Returned company entity was null");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Company_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _company.Id + 1;

            Mock<CompanyByIdQuery> query = new Mock<CompanyByIdQuery>(_unitOfWork);
            query.Setup(x => x.Execute()).Returns((Company)null);
            _serviceFactory.Setup(x => x.GetService<CompanyByIdQuery>()).Returns(query.Object);

            // Act
            try
            {
                new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(id)
                                                     .SetName("Name")
                                                     .SetPhone("555-555-5555")
                                                     .SetCity("City")
                                                     .SetState("State")
                                                     .SetZip("01234")
                                                     .SetMetroArea("Metro")
                                                     .SetIndustry("Industry")
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
        public void Not_Setting_Name_Doesnt_Change_Name_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Starting Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_Phone_Doesnt_Change_Phone_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("333-444-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_City_Doesnt_Change_City_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("Starting City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_State_Doesnt_Change_State_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("Starting State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_Zip_Doesnt_Change_Zip_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("00000", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_MetroArea_Doesnt_Change_MetroArea_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Starting Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_Industry_Doesnt_Change_Industry_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Starting Industry", result.Industry, "The created company's industry was incorrect");
        }

        [TestMethod]
        public void Not_Setting_Notes_Doesnt_Change_Notes_Value()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.IsNotNull(result, "No company was created");
            Assert.AreEqual("Name", result.Name, "The created company's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created company's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created company's city was incorrect");
            Assert.AreEqual("State", result.State, "The created company's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created company's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created company's metro area was incorrect");
            Assert.AreEqual("Starting Notes", result.Notes, "The created company's notes were incorrect");
            Assert.AreEqual("Industry", result.Industry, "The created company's industry was incorrect");
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
                new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                     .SetName("Name")
                                                     .SetPhone("555-555-5555")
                                                     .SetCity("City")
                                                     .SetState("State")
                                                     .SetZip("01234")
                                                     .SetMetroArea("Metro")
                                                     .SetIndustry("Industry")
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
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .RequestedByUserId(_user.Id)
                                                 .Execute();
            DateTime end = DateTime.Now;

            // Verify
            Company company = _unitOfWork.Companies.Fetch().Single();
            CompanyHistory history = company.History.Single();

            Assert.AreEqual("Name", history.Name, "The history record's name was incorrect");
            Assert.AreEqual("555-555-5555", history.Phone, "The history record's phone number was incorrect");
            Assert.AreEqual("City", history.City, "The history record's city was incorrect");
            Assert.AreEqual("State", history.State, "The history record's state was incorrect");
            Assert.AreEqual("01234", history.Zip, "The history record's zip was incorrect");
            Assert.AreEqual("Metro", history.MetroArea, "The history record's metro area was incorrect");
            Assert.AreEqual("Notes", history.Notes, "The history record's notes were incorrect");
            Assert.AreEqual("Industry", history.Industry, "The history record's industry was incorrect");

            Assert.AreEqual(_user, history.AuthoringUser, "The history record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryUpdate, history.HistoryAction, "The history record's action value was incorrect");
            Assert.IsTrue(history.DateModified >= start && history.DateModified <= end, "The history record's modification date was incorrect");
        }

        [TestMethod]
        public void Can_Make_Notes_More_Than_128_Characters()
        {
            // Setup
            InitializeTestEntities();
            string newNotes = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                               .RequestedByUserId(_user.Id)
                                               .SetNotes(newNotes)
                                               .Execute();

            // Verify
            Company result = _unitOfWork.Companies.Fetch().SingleOrDefault();
            Assert.AreEqual(newNotes, result.Notes, "Company's notes were not set correctly");
        }

        [TestMethod]
        public void Execute_Indexes_Edited_Company()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id).RequestedByUserId(_user.Id).Execute();

            // Verify
            _searchProvider.Verify(x => x.Index(_company), Times.Once());
        }

        [TestMethod]
        public void Can_Change_Companys_Lead_Status()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditCompanyCommand(_serviceFactory.Object).WithCompanyId(_company.Id)
                                                          .SetLeadStatus("new status")
                                                          .Execute();
            Company result = _unitOfWork.Companies.Fetch().Single();

            // Verify
            Assert.AreEqual("new status", result.LeadStatus, "Company's lead status was incorrect");
        }
    }
}
