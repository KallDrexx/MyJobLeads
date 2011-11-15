using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Users;
using MyJobLeads.DomainModel.ProcessParams.Users;

namespace MyJobLeads.DomainModel.Processes.Users
{
    public class UserPasswordProcesses 
        : IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel>,
          IProcess<VerifyUserPasswordHashParams, UserPasswordVerificationResultViewModel>
    {
        /// <summary>
        /// Generates a password for the supplied user's credentialed
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneratedPasswordHashViewModel Execute(GenerateUserPasswordHashParams procParams)
        {
            return new GeneratedPasswordHashViewModel
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SaltPassword(procParams.Email,procParams.PlainTextPassword), 20)
            };
        }

        /// <summary>
        /// Determines if the supplied plain text password and email matches the hashed password
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public UserPasswordVerificationResultViewModel Execute(VerifyUserPasswordHashParams procParams)
        {
            string saltedPass = SaltPassword(procParams.Email, procParams.PlainTextPassword);
            return new UserPasswordVerificationResultViewModel
            {
                PasswordVerified = BCrypt.Net.BCrypt.Verify(saltedPass, procParams.HashedPassword)
            };
        }

        protected string SaltPassword(string email, string plainTextPassword)
        {
            return email + plainTextPassword;
        }
    }
}
