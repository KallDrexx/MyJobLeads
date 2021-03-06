﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Queries.Users
{
    [TestClass]
    public class UserByEmailQueryTests : EFTestBase
    {
        private User _user1, _user2, _user3;

        private void InitializeTestEntities()
        {
            _user1 = new User { Email = "user1@email.com" };
            _user2 = new User { Email = "user2@email.com" };
            _user3 = new User { Email = "user3@email.com" };

            _unitOfWork.Users.Add(_user1);
            _unitOfWork.Users.Add(_user2);
            _unitOfWork.Users.Add(_user3);

            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_User_By_Email()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User result = new UserByEmailQuery(_unitOfWork).WithEmail(_user2.Email).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null user");
            Assert.AreEqual(_user2, result, "Query returned the incorrect user");
        }

        [TestMethod]
        public void Email_Query_Is_CaseInsensitive_And_Trimmed()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User result = new UserByEmailQuery(_unitOfWork).WithEmail(" USER2@EMAIL.COM ").Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null user");
            Assert.AreEqual(_user2, result, "Query returned the incorrect user");
        }

        [TestMethod]
        public void Execute_Returns_Null_User_When_No_User_Found()
        {
            // Setup
            InitializeTestEntities();
            string email = "blah@blah.com";

            // Act
            User result = new UserByEmailQuery(_unitOfWork).WithEmail(email).Execute();

            // Verify
            Assert.IsNull(result, "Execute returned a non-null user");
        }
    }
}
