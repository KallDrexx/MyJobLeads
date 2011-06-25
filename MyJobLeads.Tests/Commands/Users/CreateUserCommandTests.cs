using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;
using Moq;
using MyJobLeads.DomainModel.Queries.Users;

namespace MyJobLeads.Tests.Commands.Users
{
    [TestClass]
    public class CreateUserCommandTests : EFTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            Mock<UserByEmailQuery> userQuery = new Mock<UserByEmailQuery>(_unitOfWork);
            userQuery.Setup(x => x.Execute()).Returns((User)null);
            _serviceFactory.Setup(x => x.GetService<UserByEmailQuery>()).Returns(userQuery.Object);
        }

        [TestMethod]
        public void Can_Create_New_User()
        {
            // Setup

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Execute_Returns_Created_User()
        {
            // Setup

            // Act
            User user = new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            Assert.IsNotNull(user, "Execute returned a null user");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Email_Is_Trimmed_And_Converted_To_Lower_Case()
        {
            // Setup

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = " TEST@email.com ",
                PlainTextPassword = "password"
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_MJLDuplicateEmailException_When_Email_Already_Exists()
        {
            // Setup
            User user = new User { Email = "test@test.com" };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            Mock<UserByEmailQuery> userQuery = new Mock<UserByEmailQuery>(_unitOfWork);
            userQuery.Setup(x => x.Execute()).Returns(user);
            _serviceFactory.Setup(x => x.GetService<UserByEmailQuery>()).Returns(userQuery.Object);

            // Act
            try
            {
                new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
                {
                    Email = "test@test.com",
                    PlainTextPassword = "pass"
                });

                Assert.Fail("Command did not throw an exception");
            }

            // Catch
            catch (MJLDuplicateEmailException ex)
            {
                Assert.AreEqual("test@test.com", ex.Email, "MJLDuplicateUsernameException's email value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Initializes_JobSearch_List()
        {
            // Act
            User result = new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            Assert.IsNotNull(result.JobSearches, "User's job search list was not initialized");
        }
    }
}
