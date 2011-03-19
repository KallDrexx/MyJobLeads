using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Queries.Contacts
{
    [TestClass]
    public class ContactByIdQueryTests : EFTestBase
    {
        private Contact _contact1, _contact2, _contact3;

        private void InitializeTestEntities()
        {
            _contact1 = new Contact { Name = "contact 1" };
            _contact2 = new Contact { Name = "contact 2" };
            _contact3 = new Contact { Name = "contact 3" };

            _unitOfWork.Contacts.Add(_contact1);
            _unitOfWork.Contacts.Add(_contact2);
            _unitOfWork.Contacts.Add(_contact3);

            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_Contact_By_Id()
        {
            // Setup
            InitializeTestEntities();

            // Act
            Contact result = new ContactByIdQuery(_unitOfWork).WithContactId(_contact2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null contact");
            Assert.AreEqual(_contact2.Id, result.Id, "Returned contact had an incorrect id value");
            Assert.AreEqual(_contact2.Name, result.Name, "Returned contact had an incorrect name value");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Entity_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _contact2.Id + 100;

            // Act
            try
            {
                new ContactByIdQuery(_unitOfWork).WithContactId(id).Execute();
                Assert.Fail("Query did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Contact), ex.EntityType, "MJLEntityNotFoundException's entity type was not valid");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was not valid");
            }
        }
    }
}
