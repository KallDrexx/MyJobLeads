using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Companies;

namespace MyJobLeads.Tests.Queries.Companies
{
    [TestClass]
    public class CompaniesByJobSearchIdQueryTests : EFTestBase
    {
        private JobSearch _search1, _search2;
        private Company _company1, _company2, _company3;

        private void InitializeEntities()
        {
            _search1 = new JobSearch { Companies = new List<Company>() };
            _search2 = new JobSearch { Companies = new List<Company>() };
            _company1 = new Company { Name = "Company C" };
            _company2 = new Company { Name = "Company B" };
            _company3 = new Company { Name = "Company A" };

            _search1.Companies.Add(_company1);
            _search2.Companies.Add(_company2);
            _search1.Companies.Add(_company3);

            _unitOfWork.JobSearches.Add(_search1);
            _unitOfWork.JobSearches.Add(_search2);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Companies_For_Job_Search()
        {
            // Setup
            InitializeEntities();

            // Act
            IList<Company> results = new CompaniesByJobSearchIdQuery(_serviceFactory.Object).WithJobSearchId(_search1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(_company1.Id, results[0].Id, "First company had an incorrect id value");
            Assert.AreEqual(_company3.Id, results[1].Id, "Second company had an incorrect id value");
        }

        [TestMethod]
        public void Execute_Sorts_By_Company_Name_When_SortByName_Method_Called()
        {
            // Setup
            InitializeEntities();

            // Act
            IList<Company> results = new CompaniesByJobSearchIdQuery(_serviceFactory.Object).WithJobSearchId(_search1.Id).SortByName().Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(_company3.Name, results[0].Name, "First company was incorrect");
            Assert.AreEqual(_company1.Name, results[1].Name, "Second company was incorrect");
        }
    }
}
