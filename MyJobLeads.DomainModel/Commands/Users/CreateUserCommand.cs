using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Commands.Users
{
    /// <summary>
    /// Command class that creates a new user
    /// </summary>
    public class CreateUserCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected string _email, _password;

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
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLDuplicateUsernameException">Thrown when the requested username is already taken</exception>
        /// <exception cref="MJLDuplicateEmailException">Thrown when the requested user's email is already used on another account</exception>
        public virtual User Execute()
        {
            // Convert the email and username to lower case and trim it
            _email = _email.Trim().ToLower();

            // Check if any user is using this email already
            if (new UserByEmailQuery(_unitOfWork).WithEmail(_email).Execute() != null)
                throw new MJLDuplicateEmailException(_email);

            // Create the user
            var user = new User
            {
                Email = _email,
                Password = PasswordUtils.CreatePasswordHash(_email, _password),

                JobSearches = new List<JobSearch>()
            };

            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            return user;
        }
    }
}
