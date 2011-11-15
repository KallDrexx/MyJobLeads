using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Users;
using MyJobLeads.DomainModel.ProcessParams.Users;

namespace MyJobLeads.DomainModel.Processes.Users
{
    public class UserPasswordProcesses : IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel>
    {
        public GeneratedPasswordHashViewModel Execute(GenerateUserPasswordHashParams procParams)
        {
            return new GeneratedPasswordHashViewModel
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SaltPassword(procParams.Email,procParams.PlainTextPassword))
            };
        }

        protected string SaltPassword(string email, string plainTextPassword)
        {
            return email + plainTextPassword;
        }
    }
}
