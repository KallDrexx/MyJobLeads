using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.Data;
using Moq;

namespace MyJobLeads.Tests.Queries.Companies
{
    [TestClass]
    public class CompanyByIdQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_Company_By_Its_Id()
        {
            // Setup
            var authProcessMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            authProcessMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            Company company1 = new Company { Name = "Company 1" };
            Company company2 = new Company { Name = "Company 2" };
            Company company3 = new Company { Name = "Company 3" };

            _unitOfWork.Companies.Add(company1);
            _unitOfWork.Companies.Add(company2);
            _unitOfWork.Companies.Add(company3);

            _unitOfWork.Commit();

            // Act
            Company result = new CompanyByIdQuery(_unitOfWork, authProcessMock.Object).WithCompanyId(company2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Returned company entity was null");
            Assert.AreEqual(company2.Id, result.Id, "The returned company had an incorrect id value");
            Assert.AreEqual(company2.Name, result.Name, "The returned company had an incorrect name value");
        }

        [TestMethod]
        public void Execute_Returns_Null_Company_When_Id_Not_Found()
        {
            // Setup
            var authProcessMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            authProcessMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            Company company = new Company();
            _unitOfWork.Companies.Add(company);
            _unitOfWork.Commit();

            int id = company.Id + 1;

            // Act
            Company result = new CompanyByIdQuery(_unitOfWork, authProcessMock.Object).WithCompanyId(id).Execute();

            // Verify
            Assert.IsNull(result, "Query returned a non-null company");
        }

        [TestMethod]
        public void Execute_Returns_Null_Company_When_User_Not_Authorized()
        {
            // Setup
            var authProcessMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            authProcessMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });

            Company company = new Company();
            _unitOfWork.Companies.Add(company);
            _unitOfWork.Commit();

            // Act
            Company result = new CompanyByIdQuery(_unitOfWork, authProcessMock.Object)
                                    .WithCompanyId(company.Id)
                                    .RequestedByUserId(23)
                                    .Execute();

            // Verify
            Assert.IsNull(result, "A non-null company was returned");
            authProcessMock.Verify(x => x.Execute(It.Is<CompanyQueryAuthorizationParams>(y => y.RequestingUserId == 23)));
        }
    }
}