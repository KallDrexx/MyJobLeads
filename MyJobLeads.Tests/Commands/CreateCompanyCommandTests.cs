using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Companies;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands
{
    [TestClass]
    public class CreateCompanyCommandTests : EFTestBase
    {
        private JobSearch _search;

        private void InitializeTestEntities()
        {
            _search = new JobSearch();
            _unitOfWork.JobSearches.Add(_search);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Create_New_Company_For_Job_Search()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new CreateCompanyCommand(_unitOfWork).WithJobSearch(_search.Id)
                                                 .SetName("Name")
                                                 .SetPhone("555-555-5555")
                                                 .SetCity("City")
                                                 .SetState("State")
                                                 .SetZip("01234")
                                                 .SetMetroArea("Metro")
                                                 .SetIndustry("Industry")
                                                 .SetNotes("Notes")
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
        }

        [TestMethod]
        public void Execute_Returns_Created_Company()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Company result = new CreateCompanyCommand(_unitOfWork).WithJobSearch(_search.Id)
                                                                 .SetName("Name")
                                                                 .SetPhone("555-555-5555")
                                                                 .SetCity("City")
                                                                 .SetState("State")
                                                                 .SetZip("01234")
                                                                 .SetMetroArea("Metro")
                                                                 .SetIndustry("Industry")
                                                                 .SetNotes("Notes")
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
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_JobSearch_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _search.Id + 1;

            // Act
            try
            {
                new CreateCompanyCommand(_unitOfWork).WithJobSearch(id)
                                                     .SetName("Name")
                                                     .SetPhone("555-555-5555")
                                                     .SetCity("City")
                                                     .SetState("State")
                                                     .SetZip("01234")
                                                     .SetMetroArea("Metro")
                                                     .SetIndustry("Industry")
                                                     .SetNotes("Notes")
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
    }
}
