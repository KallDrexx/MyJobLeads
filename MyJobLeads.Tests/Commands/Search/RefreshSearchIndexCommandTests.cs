using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Search;
using Moq;
using MyJobLeads.DomainModel.Providers.Search;

namespace MyJobLeads.Tests.Commands.Search
{
    [TestClass]
    public class RefreshSearchIndexCommandTests : EFTestBase
    {
        [TestMethod]
        public void Command_Updates_Index_For_All_Entities_In_Database()
        {
            // Setup
            var mock = new Mock<ISearchProvider>();

            JobSearch search = new JobSearch();
            Company company1 = new Company { JobSearch = search }, company2 = new Company { JobSearch = search };
            Contact contact = new Contact { Company = company1 };
            Task task = new Task { Company = company2 };

            _unitOfWork.JobSearches.Add(search);
            _unitOfWork.Companies.Add(company1);
            _unitOfWork.Companies.Add(company2);
            _unitOfWork.Contacts.Add(contact);
            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Commit();

            // Act
            new RefreshSearchIndexCommand(_unitOfWork, mock.Object).Execute();

            // Verify
            mock.Verify(x => x.Index(company1), Times.Once());
            mock.Verify(x => x.Index(company2), Times.Once());
            mock.Verify(x => x.Index(contact), Times.Once());
            mock.Verify(x => x.Index(task), Times.Once());
        }
    }
}
