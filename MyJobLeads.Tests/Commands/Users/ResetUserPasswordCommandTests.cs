using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;
using Moq;

namespace MyJobLeads.Tests.Commands.Users
{
    [TestClass]
    public class ResetUserPasswordCommandTests : EFTestBase
    {
        private User _user;
        private Mock<EmailUtils> _mock;

        private void InitializeTestEntities()
        {
            _user = new User { Email = "test@email.com" };
            _unitOfWork.Users.Add(_user);
            _unitOfWork.Commit();

            _mock = new Mock<EmailUtils>();
        }

        [TestMethod]
        public void Execute_Returns_New_Password_For_User()
        {
            // Setup
            InitializeTestEntities();

            // Act
            string newPass = new ResetUserPasswordCommand(_unitOfWork, _mock.Object).WithUserEmail(_user.Email).Execute();
            User user = _unitOfWork.Users.Fetch().Single();

            // Verify
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, newPass, user.Password), "New password does not validate");
        }

        [TestMethod]
        public void Execute_Throws_MJLUserNotFoundException_When_User_Id_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            string email = "blah@blah.com";

            // Act
            try
            {
                new ResetUserPasswordCommand(_unitOfWork, _mock.Object).WithUserEmail(email).Execute();
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (MJLUserNotFoundException ex)
            {
                Assert.AreEqual(MJLUserNotFoundException.SearchPropertyType.Email, ex.SearchProperty, "MJLUserNotFoundException's search property was incorrect");
                Assert.AreEqual(email, ex.SearchValue, "MJLUserNotFoundException's search value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Sends_Email_To_User()
        {
            // Setup
            InitializeTestEntities();

            // Act
            string newPass = new ResetUserPasswordCommand(_unitOfWork, _mock.Object).WithUserEmail(_user.Email).Execute();

            // Verify
            _mock.Verify(x => x.Send(It.Is<string>(y => y == _user.Email), 
                                     It.IsAny<string>(), 
                                     It.Is<string>(y => y.Contains(newPass)),
                                     It.IsAny<bool>()), 
                        Times.Once());
        }

        [TestMethod]
        public void Command_Instantiates_EmailUtils_As_EmailProvider_When_None_Passed_In()
        {
            // Setup
            ResetUserPasswordCommand cmd = new ResetUserPasswordCommand(_unitOfWork);

            // Verify
            Assert.IsNotNull(cmd.EmailProvider, "Command's email provider was null");
            Assert.AreEqual(typeof(EmailUtils), cmd.EmailProvider.GetType(), "Command's email provider was an incorrect type");
            
        }
    }
}
