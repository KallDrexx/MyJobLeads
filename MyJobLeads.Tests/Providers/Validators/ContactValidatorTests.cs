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
    public class ContactValidatorTests
    {
        private Contact _contact;

        [TestInitialize]
        public void Setup()
        {
            _contact = new Contact
            {
                Name = "Name"
            };
        }

        [TestMethod]
        public void Correct_Contact_Passes_Validation()
        {
            // Act
            bool result = new ContactValidator().Validate(_contact).IsValid;
            
            // Verify
            Assert.IsTrue(result, "Contact was incorrectly marked as invalid");
        }

        [TestMethod]
        public void Contact_With_Empty_Name_Is_Invalid()
        {
            // Setup
            _contact.Name = " ";

            // Act
            bool result = new ContactValidator().Validate(_contact).IsValid;

            // Verify
            Assert.IsFalse(result, "Contact was incorrectly marked as valid");
        }

        [TestMethod]
        public void Contact_With_Null_Name_Is_Invalid()
        {
            // Setup
            _contact.Name = null;

            // Act
            bool result = new ContactValidator().Validate(_contact).IsValid;

            // Verify
            Assert.IsFalse(result, "Contact was incorrectly marked as valid");
        }
    }
}
