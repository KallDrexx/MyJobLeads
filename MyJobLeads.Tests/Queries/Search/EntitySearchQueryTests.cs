using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Queries.Search;

namespace MyJobLeads.Tests.Queries.Search
{
    [TestClass]
    public class EntitySearchQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Find_Result_Entities_From_Database()
        {
            // Setup
            Company company1 = new Company(), company2 = new Company();
            Contact contact = new Contact();
            Task task = new Task();

            _unitOfWork.Companies.Add(company1);
            _unitOfWork.Companies.Add(company2);
            _unitOfWork.Contacts.Add(contact);
            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Commit();

            // Result for ISearchProvider.Search() to return
            SearchProviderResult searchResult = new SearchProviderResult();
            searchResult.FoundCompanyIds.Add(company1.Id);
            searchResult.FoundCompanyIds.Add(company2.Id);
            searchResult.FoundContactIds.Add(contact.Id);
            searchResult.FoundTaskIds.Add(task.Id);

            var mock = new Mock<ISearchProvider>();
            mock.Setup(x => x.Search("Query")).Returns(searchResult);

            // Act
            SearchResultEntities result = new EntitySearchQuery(_unitOfWork, mock.Object).WithSearchQuery("Query").Execute();

            // Verify
            Assert.IsNotNull(result, "Search provider returned a null result");
            Assert.AreEqual(2, result.Companies.Count, "Incorrect number of companies was returned");
            Assert.AreEqual(1, result.Contacts.Count, "Incorrect number of contacts was returned");
            Assert.AreEqual(1, result.Tasks.Count, "incorrect number of tasks was returned");
            Assert.IsTrue(result.Companies.Contains(company1), "Returned company list did not contain the first company");
            Assert.IsTrue(result.Companies.Contains(company2), "Returned company list did not conain the second company");
            Assert.AreEqual(contact, result.Contacts[0], "Returned contact was incorrect");
            Assert.AreEqual(task, result.Tasks[0], "Returned task was incorrect");
        }

        [TestMethod]
        public void Does_Not_Return_Entities_Not_In_Search_Results()
        {
            // Setup
            Company company = new Company(), company2 = new Company();
            Contact contact = new Contact(), contact2 = new Contact();
            Task task = new Task(), task2 = new Task();

            _unitOfWork.Companies.Add(company);
            _unitOfWork.Companies.Add(company2);
            _unitOfWork.Contacts.Add(contact);
            _unitOfWork.Contacts.Add(contact2);
            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Tasks.Add(task2);
            _unitOfWork.Commit();

            // Result for ISearchProvider.Search() to return
            SearchProviderResult searchResult = new SearchProviderResult();
            searchResult.FoundCompanyIds.Add(company.Id);
            searchResult.FoundContactIds.Add(contact.Id);
            searchResult.FoundTaskIds.Add(task.Id);

            var mock = new Mock<ISearchProvider>();
            mock.Setup(x => x.Search("Query")).Returns(searchResult);

            // Act
            SearchResultEntities result = new EntitySearchQuery(_unitOfWork, mock.Object).WithSearchQuery("Query").Execute();

            // Verify
            Assert.IsNotNull(result, "Search results returned a null result");
            Assert.IsFalse(result.Companies.Contains(company2), "Search results incorrectly contained the 2nd company");
            Assert.IsFalse(result.Contacts.Contains(contact2), "Search results incorrectly contained the 2nd contact");
            Assert.IsFalse(result.Tasks.Contains(task2), "Search results incorrectly contained the 2nd task");
        }
    }
}
