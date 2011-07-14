using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers.Validation;

namespace MyJobLeads.Tests.Providers.Validators
{
    [TestClass]
    public class CompanyValidatorTests
    {
        private Company _company;

        [TestInitialize]
        public void InitializeEntities()
        {
            _company = new Company
            {
                Name = "name"
            };
        }

        [TestMethod]
        public void Company_Can_Be_Valid()
        {
            // Act
            bool result = new CompanyValidator().Validate(_company).IsValid;

            // Verify
            Assert.IsTrue(result, "Company was incorrectly marked as invalid");
        }

        [TestMethod]
        public void Company_With_Empty_Name_Is_Invalid()
        {
            // Setup
            _company.Name = " ";

            // Act
            bool result = new CompanyValidator().Validate(_company).IsValid;
            
            // Verify
            Assert.IsFalse(result, "Company was incorrectly marked as valid");
        }

        [TestMethod]
        public void Company_With_Null_Name_Is_Invalid()
        {
            // Setup
            _company.Name = null;

            // Act
            bool result = new CompanyValidator().Validate(_company).IsValid;

            // Verify
            Assert.IsFalse(result, "Company was incorrectly marked as valid");
        }
    }
}
