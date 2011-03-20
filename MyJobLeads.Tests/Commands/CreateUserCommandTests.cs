using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.Tests.Commands
{
    [TestClass]
    public class CreateUserCommandTests : EFTestBase
    {
        [TestMethod]
        public void Can_Create_New_User()
        {
            // Setup

            // Act
            new CreateUserCommand(_unitOfWork).SetEmail("test@email.com")
                                              .SetPassword("password")
                                              .SetDisplayName("Test Account")
                                              .Execute();

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.AreEqual("Test Account", user.DisplayName, "User's display name was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Execute_Returns_Created_User()
        {
            // Setup

            // Act
            User user = new CreateUserCommand(_unitOfWork).SetEmail("test@email.com")
                                                          .SetPassword("password")
                                                          .SetDisplayName("Test Account")
                                                          .Execute();

            // Verify
            Assert.IsNotNull(user, "Execute returned a null user");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.AreEqual("Test Account", user.DisplayName, "User's display name was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Username_Is_Trimmed_And_Converted_To_Lower_Case()
        {
            // Setup


            // Act
            new CreateUserCommand(_unitOfWork).SetEmail(" TEST@email.com ")
                                              .SetPassword("password")
                                              .SetDisplayName("Test Account")
                                              .Execute();

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.AreEqual("Test Account", user.DisplayName, "User's display name was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }
    }
}
