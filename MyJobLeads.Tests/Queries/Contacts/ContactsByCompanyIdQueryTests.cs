using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;

namespace MyJobLeads.Tests.Queries.Contacts
{
    [TestClass]
    public class ContactsByCompanyIdQueryTests : EFTestBase
    {
        private Company _company1, _company2;
        private Contact _contact1, _contact2, _contact3;

        private void InitializeEntities()
        {
            _company1 = new Company { Contacts = new List<Contact>() };
            _company2 = new Company { Contacts = new List<Contact>() };
            _contact1 = new Contact();
            _contact2 = new Contact();
            _contact3 = new Contact();

            _company1.Contacts.Add(_contact1);
            _company2.Contacts.Add(_contact2);
            _company1.Contacts.Add(_contact3);

            _unitOfWork.Companies.Add(_company1);
            _unitOfWork.Companies.Add(_company2);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Contacts_For_Companies()
        {
            // Setup
            InitializeEntities();

            // Act
            IList<Contact> results = new ContactsByCompanyIdQuery(_unitOfWork).WithCompanyId(_company1.Id).Execute();

            // Verify
            Assert.IsNotNull(results, "Query returned a null list");
            Assert.AreEqual(2, results.Count, "Returned list had an incorrect number of elements");
            Assert.AreEqual(_contact1.Id, results[0].Id, "First contact had an incorrect id value");
            Assert.AreEqual(_contact3.Id, results[1].Id, "Second contact had an incorrect id value");
        }
    }
}
