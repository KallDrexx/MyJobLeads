using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.Tests.Queries.Users
{
    [TestClass]
    public class UserByCredentialsQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Retrieve_User_With_Correct_Credentials()
        {
            // Setup
            User user1 = new User { Email = "user1", Password = PasswordUtils.CreatePasswordHash("user1", "pass1") };
            User user2 = new User { Email = "user2", Password = PasswordUtils.CreatePasswordHash("user2", "pass2") };
            User user3 = new User { Email = "user3", Password = PasswordUtils.CreatePasswordHash("user3", "pass3") };
            _unitOfWork.Users.Add(user1);
            _unitOfWork.Users.Add(user2);
            _unitOfWork.Users.Add(user3);
            _unitOfWork.Commit();

            // Act
            User result = new UserByCredentialsQuery(_unitOfWork).WithEmail("user2")
                                                                 .WithPassword("pass2")
                                                                 .Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null user");
            Assert.AreEqual(user2.Id, result.Id, "Query returned the wrong user");
            Assert.AreEqual(user2.Email, result.Email, "Query returned a user with an incorrect email");
        }

        [TestMethod]
        public void Incorrect_Email_Returns_Null_User()
        {
            // Setup
            User user = new User { Email = "user", Password = PasswordUtils.CreatePasswordHash("user", "pass") };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            // Act
            User result = new UserByCredentialsQuery(_unitOfWork).WithEmail("username")
                                                                 .WithPassword("pass")
                                                                 .Execute();

            // Verify
            Assert.IsNull(result, "Query returned a valid user, when none should have been returned");
        }

        [TestMethod]
        public void Incorrect_Password_Returns_Null_User()
        {
            // Setup
            User user = new User { Email = "user", Password = PasswordUtils.CreatePasswordHash("user", "pass") };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            // Act
            User result = new UserByCredentialsQuery(_unitOfWork).WithEmail("user")
                                                                 .WithPassword("password")
                                                                 .Execute();

            // Verify
            Assert.IsNull(result, "Query returned a valid user, when none should have been returned");
        }

        [TestMethod]
        public void Username_Supplied_Is_Case_Insensitive_And_Trimmed()
        {
            // Setup
            User user1 = new User { Email = "user1", Password = PasswordUtils.CreatePasswordHash("user1", "pass1") };
            User user2 = new User { Email = "user2", Password = PasswordUtils.CreatePasswordHash("user2", "pass2") };
            User user3 = new User { Email = "user3", Password = PasswordUtils.CreatePasswordHash("user3", "pass3") };
            _unitOfWork.Users.Add(user1);
            _unitOfWork.Users.Add(user2);
            _unitOfWork.Users.Add(user3);
            _unitOfWork.Commit();

            // Act
            User result = new UserByCredentialsQuery(_unitOfWork).WithEmail(" USER2 ")
                                                                 .WithPassword("pass2")
                                                                 .Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null user");
            Assert.AreEqual(user2.Id, result.Id, "Query returned the wrong user");
            Assert.AreEqual(user2.Email, result.Email, "Query returned a user with an incorrect email");
        }
    }
}
