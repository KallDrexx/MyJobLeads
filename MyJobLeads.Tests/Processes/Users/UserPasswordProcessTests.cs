using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.Users;
using MyJobLeads.DomainModel.ViewModels.Users;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Users;

namespace MyJobLeads.Tests.Processes.Users
{
    [TestClass]
    public class UserPasswordProcessTests : EFTestBase
    {
        [TestMethod]
        public void Can_Generate_BCrypt_Password_Hash_Salted_With_Email_Concatenated_With_Password()
        {
            // Setup
            IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel> process = new UserPasswordProcesses();
            string email = "email", password = "password";
            string saltedPassword = email + password;

            // Act
            var result = process.Execute(new GenerateUserPasswordHashParams { Email = email, PlainTextPassword = password });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(saltedPassword, result.PasswordHash), "Password has was not correct");
        }

        [TestMethod]
        public void Can_Verify_BCrypt_Password_Hash_Salted_With_Email_Concatenated_With_Password()
        {
            // Setup
            IProcess<VerifyUserPasswordHashParams, UserPasswordVerificationResultViewModel> process = new UserPasswordProcesses();
            string email = "email", password = "password";
            string saltedPassword = email + password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword);

            // Act
            var result = process.Execute(new VerifyUserPasswordHashParams { Email = email, PlainTextPassword = password, HashedPassword = hashedPassword });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.PasswordVerified, "Password was not verified as matching");
        }

        [TestMethod]
        public void Verify_Fails_For_BCrypt_Password_Hash_Not_Salted()
        {
            // Setup
            IProcess<VerifyUserPasswordHashParams, UserPasswordVerificationResultViewModel> process = new UserPasswordProcesses();
            string email = "email", password = "password";
            string saltedPassword = email + password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Act
            var result = process.Execute(new VerifyUserPasswordHashParams { Email = email, PlainTextPassword = password, HashedPassword = hashedPassword });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.PasswordVerified, "Password was incorrectly verified as matching");
        }

        [TestMethod]
        public void Verify_Fails_For_BCrypt_Password_Hash_Not_Correct_For_Supplied_Credentials()
        {
            // Setup
            IProcess<VerifyUserPasswordHashParams, UserPasswordVerificationResultViewModel> process = new UserPasswordProcesses();
            string email = "email", password = "password";
            string saltedPassword = email + password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword);

            // Act
            var result = process.Execute(new VerifyUserPasswordHashParams { Email = email, PlainTextPassword = "incorrect pass", HashedPassword = hashedPassword });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.PasswordVerified, "Password was incorrectly verified as matching");
        }
    }
}
