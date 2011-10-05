using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using Moq;

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
            var authMock = new Mock<IProcess<ContactAutorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<ContactAutorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            // Act
            Contact result = new ContactByIdQuery(_unitOfWork, authMock.Object).WithContactId(_contact2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null contact");
            Assert.AreEqual(_contact2.Id, result.Id, "Returned contact had an incorrect id value");
            Assert.AreEqual(_contact2.Name, result.Name, "Returned contact had an incorrect name value");
        }

        [TestMethod]
        public void Execute_Returns_Null_Contact_When_Id_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            var authMock = new Mock<IProcess<ContactAutorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<ContactAutorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            int id = _contact2.Id + 100;

            // Act
            Contact result = new ContactByIdQuery(_unitOfWork, authMock.Object).WithContactId(id).Execute();

            // Verify
            Assert.IsNull(result, "Query returned a non-null contact");
        }

        [TestMethod]
        public void Execute_Returns_Null_Contact_When_User_Not_Authorized()
        {
            // Setup
            InitializeTestEntities();
            var authMock = new Mock<IProcess<ContactAutorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<ContactAutorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });

            // Act
            Contact result = new ContactByIdQuery(_unitOfWork, authMock.Object)
                                    .WithContactId(_contact2.Id)
                                    .RequestedByUserId(15)
                                    .Execute();

            // Verify
            Assert.IsNull(result, "Query returned a non-null contact");
            authMock.Verify(x => x.Execute(It.Is<ContactAutorizationParams>(y => y.RequestingUserId == 15)));
        }
    }
}
