using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.Users
{
    [TestClass]
    public class CreateUserCommandTests : EFTestBase
    {
        [TestMethod]
        public void Can_Create_New_User()
        {
            // Setup

            // Act
            new CreateUserCommand(_unitOfWork).SetUsername("username")
                                              .SetEmail("test@email.com")
                                              .SetPassword("password")
                                              .Execute();

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("username", user.Username, "User's username was incorrect");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Username, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Execute_Returns_Created_User()
        {
            // Setup

            // Act
            User user = new CreateUserCommand(_unitOfWork).SetEmail("test@email.com")
                                                          .SetPassword("password")
                                                          .SetUsername("username")
                                                          .Execute();

            // Verify
            Assert.IsNotNull(user, "Execute returned a null user");
            Assert.AreEqual("username", user.Username, "User's username was incorrect");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Username, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Email_Is_Trimmed_And_Converted_To_Lower_Case()
        {
            // Setup

            // Act
            new CreateUserCommand(_unitOfWork).SetEmail(" TEST@email.com ")
                                              .SetPassword("password")
                                              .SetUsername("username")
                                              .Execute();

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
        }

        [TestMethod]
        public void Username_Is_Trimmed_And_Converted_To_Lower_Case()
        {
            // Setup

            // Act
            new CreateUserCommand(_unitOfWork).SetEmail("TEST@email.com")
                                              .SetPassword("password")
                                              .SetUsername(" UserName")
                                              .Execute();

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("username", user.Username, "User's username was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_MJLDuplicateUsernameException_When_Username_Already_Exists()
        {
            // Setup
            User user = new User { Username = "user" };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            // Act
            try
            {
                new CreateUserCommand(_unitOfWork).SetUsername("user")
                                                  .SetPassword("pass")
                                                  .SetEmail("blah@blah.com")
                                                  .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Catch
            catch (MJLDuplicateUsernameException ex)
            {
                Assert.AreEqual("user", ex.Username, "MJLDuplicateUsernameException's username value was incorrect");
            }
        }
    }
}
