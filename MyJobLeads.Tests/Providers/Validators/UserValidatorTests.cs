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
    public class UserValidatorTests
    {
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User
            {
                Email = "test@test.com"
            };
        }

        [TestMethod]
        public void Correct_User_Is_Valid()
        {
            // Act
            bool result = new UserValidator().Validate(_user).IsValid;

            // Verify
            Assert.IsTrue(result, "User was not marked as valid");
        }

        [TestMethod]
        public void Non_Email_Value_Marks_User_As_Invalid()
        {
            // Setup
            _user.Email = "testing";

            // Act
            bool result = new UserValidator().Validate(_user).IsValid;

            // Verify
            Assert.IsFalse(result, "User was incorrectly marked as valid");
        }
    }
}
