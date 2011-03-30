using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Commands.Users
{
    [TestClass]
    public class ResetUserPasswordCommandTests : EFTestBase
    {
        private User _user;

        private void InitializeTestEntities()
        {
            _user = new User { Username = "username", Email = "test@email.com" };
            _unitOfWork.Users.Add(_user);
            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Execute_Returns_New_Password_For_User()
        {
            // Setup
            InitializeTestEntities();

            // Act
            string newPass = new ResetUserPasswordCommand(_unitOfWork).WithUserId(_user.Id).Execute();
            User user = _unitOfWork.Users.Fetch().Single();

            // Verify
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Username, newPass, user.Password), "New password does not validate");
        }

        [TestMethod]
        public void Execute_Throws_MJLEntityNotFoundException_When_User_Id_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            int id = _user.Id + 101;

            // Act
            try
            {
                new ResetUserPasswordCommand(_unitOfWork).WithUserId(id).Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}
