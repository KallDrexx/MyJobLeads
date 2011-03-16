using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Queries.Companies
{
    [TestClass]
    public class CompanyByIdQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_Company_By_Its_Id()
        {
            // Setup
            Company company1 = new Company { Name = "Company 1" };
            Company company2 = new Company { Name = "Company 2" };
            Company company3 = new Company { Name = "Company 3" };

            _unitOfWork.Companies.Add(company1);
            _unitOfWork.Companies.Add(company2);
            _unitOfWork.Companies.Add(company3);

            _unitOfWork.Commit();

            // Act
            Company result = new CompanyByIdQuery(_unitOfWork).WithCompanyId(company2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Returned company entity was null");
            Assert.AreEqual(company2.Id, result.Id, "The returned company had an incorrect id value");
            Assert.AreEqual(company2.Name, result.Name, "The returned company had an incorrect name value");
        }

        [TestMethod]
        public void Execute_Throws_Exception_When_Company_Not_Found()
        {
            // Setup
            Company company = new Company();
            _unitOfWork.Companies.Add(company);
            _unitOfWork.Commit();

            int id = company.Id + 1;

            // Act
            try
            {
                Company result = new CompanyByIdQuery(_unitOfWork).WithCompanyId(id).Execute();
                Assert.Fail("Query did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Company), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}