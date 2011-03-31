﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.Users
{
    [TestClass]
    public class EditUserCommandTests : EFTestBase
    {
        private User _user;
        private const string _oldPassword = "starting password", _startingEmail = "starting@email.com";

        private void InitializeTestEntities()
        {
            _user = new User
            {
                Email = _startingEmail ,
                Password = PasswordUtils.CreatePasswordHash(_startingEmail, _oldPassword),
                LastVisitedJobSearchId = 2
            };

            _unitOfWork.Users.Add(_user);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Edit_User_Entity()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                            .SetEmail("new@email.com")
                                            .SetPassword("new password")
                                            .SetLastVisitedJobSearchId(5)
                                            .WithExistingPassword(_oldPassword)
                                            .Execute();
            User result = _unitOfWork.Users.Fetch().SingleOrDefault();

            // Verify
            Assert.IsNotNull(result, "No user was found in the repository");
            Assert.AreEqual(_user, result, "User was incorrect");
            Assert.AreEqual("new@email.com", result.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash("new@email.com", "new password", result.Password), "User's password was incorrect");
            Assert.AreEqual(5, result.LastVisitedJobSearchId, "User's last visited jobsearch id was incorrect");
        }

        [TestMethod]
        public void Execute_Returns_Editted_User()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User result = new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                                        .SetEmail("new@email.com")
                                                        .SetPassword("new password")
                                                        .SetLastVisitedJobSearchId(5)
                                                        .WithExistingPassword(_oldPassword)
                                                        .Execute();

            // Verify
            Assert.IsNotNull(result, "No user was found in the repository");
            Assert.AreEqual(_user, result, "User was incorrect");
            Assert.AreEqual("new@email.com", result.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash("new@email.com", "new password", result.Password), "User's password was incorrect");
            Assert.AreEqual(5, result.LastVisitedJobSearchId, "User's last visited jobsearch id was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 101;

            // Act
            try
            {
                new EditUserCommand(_unitOfWork).WithUserId(id)
                                                .SetEmail("new@email.com")
                                                .SetPassword("new password")
                                                .Execute();
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Email_Is_Not_Changed_When_Not_Set()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                            .SetPassword("new password")
                                            .SetLastVisitedJobSearchId(5)
                                            .WithExistingPassword(_oldPassword)
                                            .Execute();
            User result = _unitOfWork.Users.Fetch().SingleOrDefault();

            // Verify
            Assert.IsNotNull(result, "No user was found in the repository");
            Assert.AreEqual(_user, result, "User was incorrect");
            Assert.AreEqual("starting@email.com", result.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(result.Email, "new password", result.Password), "User's password was incorrect");
            Assert.AreEqual(5, result.LastVisitedJobSearchId, "User's last visited jobsearch id was incorrect");
        }

        [TestMethod]
        public void Password_Is_Not_Changed_When_Not_Specified()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                            .SetEmail("new@email.com")
                                            .SetLastVisitedJobSearchId(5)
                                            .WithExistingPassword(_oldPassword)
                                            .Execute();
            User result = _unitOfWork.Users.Fetch().SingleOrDefault();
            
            // Verify
            Assert.IsNotNull(result, "No user was found in the repository");
            Assert.AreEqual(_user, result, "User was incorrect");
            Assert.AreEqual("new@email.com", result.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(result.Email, "starting password", result.Password), "User's password was incorrect");
            Assert.AreEqual(5, result.LastVisitedJobSearchId, "User's last visited jobsearch id was incorrect");
        }

        [TestMethod]
        public void LastVisitedJobSearchId_Is_Not_Changed_When_Not_Specified()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                            .SetEmail("new@email.com")
                                            .SetPassword("new password")
                                            .WithExistingPassword(_oldPassword)
                                            .Execute();
            User result = _unitOfWork.Users.Fetch().SingleOrDefault();

            // Verify
            Assert.IsNotNull(result, "No user was found in the repository");
            Assert.AreEqual(_user, result, "User was incorrect");
            Assert.AreEqual("new@email.com", result.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(result.Email, "new password", result.Password), "User's password was incorrect");
            Assert.AreEqual(2, result.LastVisitedJobSearchId, "User's last visited jobsearch id was incorrect");
        }

        [TestMethod]
        public void LastVisitedJobSearchId_Can_Be_Edited_Without_Existing_Password()
        {
            // Setup
            InitializeTestEntities();

            // Act
            new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                            .SetLastVisitedJobSearchId(6)
                                            .Execute();
            User result = _unitOfWork.Users.Fetch().SingleOrDefault();

            // Verify
            Assert.AreEqual(6, result.LastVisitedJobSearchId, "LastVisitedJobSearchId value was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_MJLIncorrectPasswordException_When_Existing_Password_Not_Given()
        {
            // Setup
            InitializeTestEntities();

            // Act
            try
            {
                new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                                .SetEmail("new@email.com")
                                                .SetLastVisitedJobSearchId(5)
                                                .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLIncorrectPasswordException ex)
            {
                Assert.AreEqual(_user.Id, ex.UserId, "MJLIncorrectPasswordException's user id value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Throws_MJLIncorrectPasswordException_When_Existing_Password_Is_Incorrect()
        {
            // Setup
            InitializeTestEntities();

            // Act
            try
            {
                new EditUserCommand(_unitOfWork).WithUserId(_user.Id)
                                                .SetEmail("new@email.com")
                                                .SetLastVisitedJobSearchId(5)
                                                .WithExistingPassword("wrong password")
                                                .Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLIncorrectPasswordException ex)
            {
                Assert.AreEqual(_user.Id, ex.UserId, "MJLIncorrectPasswordException's user id value was incorrect");
            }
        }
    }
}