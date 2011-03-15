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
        public void Execute_Throws_Exception_When_User_Not_Found()
        {
            // Setup
            InitializeTestEntities();

            // Act
            try
            {
                new UserByIdQuery(_unitOfWork).WithUserId(-1).Execute();
                Assert.Fail("Query did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual((-1).ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}
