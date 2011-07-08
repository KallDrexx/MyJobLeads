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
using Moq;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Commands.JobSearches;

namespace MyJobLeads.Tests.Commands.Companies
{
    [TestClass]
    public class CreateCompanyCommandTests : EFTestBase
    {
        private JobSearch _search;
        private User _user;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<UpdateJobSearchMetricsCommand> _updateMetricsCmd;

        private void InitializeTestEntities()
        {
            _search = new JobSearch();
            _user = new User();

            _unitOfWork.Users.Add(_user);
            _unitOfWork.JobSearches.Add(_search);
            _unitOfWork.Commit();

            // Initialize Mocks
            _searchProvider = new Mock<ISearchProvider>();
            _serviceFactory.Setup(x => x.GetService<ISearchProvider>()).Returns(_searchProvider.Object);

            var userByIdQuery = new Mock<UserByIdQuery>(_unitOfWork);
            userByIdQuery.Setup(x => x.Execute()).Returns(_user);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(userByIdQuery.Object);

            var jobsearchByIdQuery = new Mock<JobSearchByIdQuery>(_unitOfWork);
            jobsearchByIdQuery.Setup(x => x.Execute()).Returns(_search);
            _serviceFactory.Setup(x => x.GetService<JobSearchByIdQuery>()).Returns(jobsearchByIdQuery.Object);

            _updateMetricsCmd = new Mock<UpdateJobSearchMetricsCommand>(_serviceFactory.Object);
            _serviceFactory.Setup(x => x.GetService<UpdateJobSearchMetricsCommand>()).Returns(_updateMetricsCmd.Object);
        }

        [TestMethod]
        public void Can_Create_New_Company_For_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .CalledByUserId(_user.Id)
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
            Assert.AreEqual(_search, result.JobSearch, "The created company was associated with the incorrect job search");
        }

        [TestMethod]
        public void Execute_Returns_Created_Company()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company result = new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id)
                                                                 .SetName("Name")
                                                                 .SetPhone("555-555-5555")
                                                                 .SetCity("City")
                                                                 .SetState("State")
                                                                 .SetZip("01234")
                                                                 .SetMetroArea("Metro")
                                                                 .SetIndustry("Industry")
                                                                 .SetNotes("Notes")
                                                                 .CalledByUserId(_user.Id)
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
            Assert.AreEqual(_search, result.JobSearch, "The created company was associated with the incorrect job search");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_JobSearch_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _search.Id + 1;

            var query = new Mock<JobSearchByIdQuery>(_unitOfWork);
            query.Setup(x => x.Execute()).Returns((JobSearch)null);
            _serviceFactory.Setup(x => x.GetService<JobSearchByIdQuery>()).Returns(query.Object);

            // Act
            try
            {
                new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(id)
                                                     .SetName("Name")
                                                     .SetPhone("555-555-5555")
                                                     .SetCity("City")
                                                     .SetState("State")
                                                     .SetZip("01234")
                                                     .SetMetroArea("Metro")
                                                     .SetIndustry("Industry")
                                                     .SetNotes("Notes")
                                                     .CalledByUserId(_user.Id)
                                                     .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(JobSearch), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Initializes_Task_List()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company result = new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id).CalledByUserId(_user.Id).Execute();

            // Verify
            Assert.IsNotNull(result.Tasks, "Company's task list was null");
        }

        [TestMethod]
        public void Execute_Initializes_Contact_List()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company result = new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id).CalledByUserId(_user.Id).Execute();

            // Verify
            Assert.IsNotNull(result.Contacts, "Company's contact list was null");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_Calling_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 101;

            var query = new Mock<UserByIdQuery>(_unitOfWork);
            query.Setup(x => x.Execute()).Returns((User)null);
            _serviceFactory.Setup(x => x.GetService<UserByIdQuery>()).Returns(query.Object);

            // Act
            try
            {
                new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id)
                                                     .SetName("Name")
                                                     .SetPhone("555-555-5555")
                                                     .SetCity("City")
                                                     .SetState("State")
                                                     .SetZip("01234")
                                                     .SetMetroArea("Metro")
                                                     .SetIndustry("Industry")
                                                     .SetNotes("Notes")
                                                     .CalledByUserId(id)
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
            new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
                                                 .CalledByUserId(_user.Id)
                                                 .Execute();
            DateTime end = DateTime.Now;

            // Verify
            Company company = _unitOfWork.Companies.Fetch().Single();
            CompanyHistory result = company.History.Single();

            Assert.IsNotNull(result, "No history record was created");
            Assert.AreEqual("Name", result.Name, "The created history record's name was incorrect");
            Assert.AreEqual("555-555-5555", result.Phone, "The created history record's phone number was incorrect");
            Assert.AreEqual("City", result.City, "The created history record's city was incorrect");
            Assert.AreEqual("State", result.State, "The created history record's state was incorrect");
            Assert.AreEqual("01234", result.Zip, "The created history record's zip was incorrect");
            Assert.AreEqual("Metro", result.MetroArea, "The created history record's metro area was incorrect");
            Assert.AreEqual("Notes", result.Notes, "The created history record's notes were incorrect");
            Assert.AreEqual(_user, result.AuthoringUser, "The history record's author was incorrect");
            Assert.AreEqual(MJLConstants.HistoryInsert, result.HistoryAction, "The history record's history action was incorrect");
            Assert.IsTrue(result.DateModified >= start && result.DateModified <= end, "The history record's modification date was incorrect");
        }

        [TestMethod]
        public void Execute_Indexes_New_Company()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company company = new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id)
                                                                                           .CalledByUserId(_user.Id)
                                                                                           .Execute();

            // Verify
            _searchProvider.Verify(x => x.Index(company), Times.Once());
        }

        [TestMethod]
        public void Command_Updates_JobSearch_Metrics()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company company = new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id)
                                                                                           .CalledByUserId(_user.Id)
                                                                                           .Execute();

            // Verify
            _updateMetricsCmd.Verify(x => x.Execute(It.Is<UpdateJobSearchMetricsCmdParams>(y => y.JobSearchId == _search.Id)), Times.Once());
        }

        [TestMethod]
        public void New_Company_Has_Prospective_Employer_Status()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateCompanyCommand(_serviceFactory.Object).WithJobSearch(_search.Id).Execute();
            Company result = _unitOfWork.Companies.Fetch().Single();

            // Verify
            Assert.AreEqual(MJLConstants.ProspectiveEmployerCompanyStatus, result.LeadStatus, "Company's status was incorrect");
        }
    }
}
