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
    public class UserByIdQueryTests : EFTestBase
    {
        private User _user;

        public void InitializeTestEntities()
        {
            _user = new User { JobSearches = new List<JobSearch>() };
            _unitOfWork.Users.Add(new User());
            _unitOfWork.Users.Add(_user);
            _unitOfWork.Users.Add(new User());
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Retrieve_User_By_Id()
        {
            // Setup
            InitializeTestEntities();

            // Act
            User result = new UserByIdQuery(_unitOfWork).WithUserId(_user.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "User returned was null");
            Assert.AreEqual(result.Id, _user.Id, "User returned had an incorrect id value");
        }

        [TestMethod]
        public void Execute_Returns_Null_User_When_UserId_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 100;

            // Act
            User result = new UserByIdQuery(_unitOfWork).WithUserId(id).Execute();

            // Verify
            Assert.IsNull(result, "Query returned a non-null user");
        }
    }
}
