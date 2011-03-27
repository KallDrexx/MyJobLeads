using System;
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
    public class UserByUsernameQueryTests : EFTestBase
    {
        private User _user1, _user2, _user3;

        private void InitializeTestEntities()
        {
            _user1 = new User { Username = "user1" };
            _user2 = new User { Username = "user2" };
            _user3 = new User { Username = "user3" };

            _unitOfWork.Users.Add(_user1);
            _unitOfWork.Users.Add(_user2);
            _unitOfWork.Users.Add(_user3);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_User_By_Username()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User user = new UserByUsernameQuery(_unitOfWork).WithUsername("user2").Execute();

            // Verify
            Assert.AreEqual(_user2, user, "Query returned an incorrect user entity");
        }

        [TestMethod]
        public void Can_Retrieve_User_With_CaseInsensitive_And_Non_Trimmed_Username()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User user = new UserByUsernameQuery(_unitOfWork).WithUsername(" USER2 ").Execute();

            // Verify
            Assert.AreEqual(_user2, user, "Query returned an incorrect user entity");
        }

        [TestMethod]
        public void Execute_Returns_Null_User_When_Username_Not_Found()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User user = new UserByUsernameQuery(_unitOfWork).WithUsername("username").Execute();

            // Verify
            Assert.IsNull(user, "Query returned a non-null user");
        }
    }
}
