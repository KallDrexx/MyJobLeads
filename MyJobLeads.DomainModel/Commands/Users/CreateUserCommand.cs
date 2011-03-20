using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.DomainModel.Commands.Users
{
    /// <summary>
    /// Command class that creates a new user
    /// </summary>
    public class CreateUserCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected string _email, _password, _username;

        public CreateUserCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the email of the new user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public CreateUserCommand SetEmail(string email)
        {
            _email = email;
            return this;
        }

        /// <summary>
        /// Specifies the password for the new user
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public CreateUserCommand SetPassword(string password)
        {
            _password = password;
            return this;
        }

        /// <summary>
        /// Specifies the username for the new user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public CreateUserCommand SetUsername(string username)
        {
            _username = username;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        public User Execute()
        {
            // Convert the email and username to lower case and trim it
            _username = _username.Trim().ToLower();
            _email = _email.Trim().ToLower();

            // Create the user
            var user = new User
            {
                Username = _username,
                Email = _email,
                Password = PasswordUtils.CreatePasswordHash(_username, _password)
            };

            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            return user;
        }
    }
}
