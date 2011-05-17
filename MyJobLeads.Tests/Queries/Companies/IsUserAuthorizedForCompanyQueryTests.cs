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
    public class IsUserAuthorizedForCompanyQueryTests : EFTestBase
    {
        [TestMethod]
        public void User_That_Created_Company_Is_Authorized()
        {
            // Setup
            User user = new User();
            Company company = new Company { CreatedByUser = user };

            _unitOfWork.Companies.Add(company);
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            // Act
            bool result = new IsUserAuthorizedForCompanyQuery(_unitOfWork).WithUserId(user.Id).WithCompanyId(company.Id).Execute();

            // Verify
            Assert.IsTrue(result, "User was incorrectly not authorized for the specified company");
        }

        [TestMethod]
        public void User_That_Didnt_Create_Company_Is_Not_Authorized()
        {
            // Setup
            User user = new User(), user2 = new User();
            Company company = new Company { CreatedByUser = user };

            _unitOfWork.Companies.Add(company);
            _unitOfWork.Users.Add(user);
            _unitOfWork.Users.Add(user2);
            _unitOfWork.Commit();

            // Act
            bool result = new IsUserAuthorizedForCompanyQuery(_unitOfWork).WithUserId(user2.Id).WithCompanyId(company.Id).Execute();

            // Verify
            Assert.IsFalse(result, "User was incorrectly not authorized for the specified company");
        }
    }
}
