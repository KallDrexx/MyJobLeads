using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.Tests.Utilities
{
    [TestClass]
    public class PasswordUtilsTests
    {
        [TestMethod]
        public void Correct_User_Credentials_Passes_Password_Check()
        {
            // Setup
            string username = "user name";
            string password = "passWord";
            string hash = "$2a$15$qPHP3aj9z3j/6f/4BGwuUeuIgWYqidQ/OxrXEayXVoc1RC5s9rLse";

            // Act
            bool result = PasswordUtils.CheckPasswordHash(username, password, hash);

            // Verify 
            Assert.IsTrue(result, "Password check failed");
        }

        [TestMethod]
        public void Incorrect_User_Credentials_Fails_Password_Check()
        {
            // Setup
            string username = "user name";
            string password = "password";
            string hash = "$2a$15$qPHP3aj9z3j/6f/4BGwuUeuIgWYqidQ/OxrXEayXVoc1RC5s9rLse";

            // Act
            bool result = PasswordUtils.CheckPasswordHash(username, password, hash);

            // Verify 
            Assert.IsFalse(result, "Password check passed but should have failed");
        }
    }
}
