using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Providers;

namespace MyJobLeads.DomainModel.Commands.Users
{
    public struct CreateUserCommandParams
    {
        public string Email { get; set; }
        public string PlainTextPassword { get; set; }
    }

    /// <summary>
    /// Command class that creates a new user
    /// </summary>
    public class CreateUserCommand
    {
        protected IServiceFactory _serviceFactory;

        public CreateUserCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLDuplicateUsernameException">Thrown when the requested username is already taken</exception>
        /// <exception cref="MJLDuplicateEmailException">Thrown when the requested user's email is already used on another account</exception>
        public virtual User Execute(CreateUserCommandParams cmdParams)
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Convert the email and username to lower case and trim it
            cmdParams.Email = cmdParams.Email.Trim().ToLower();

            // Check if any user is using this email already
            if (_serviceFactory.GetService<UserByEmailQuery>().WithEmail(cmdParams.Email).Execute() != null)
                throw new MJLDuplicateEmailException(cmdParams.Email);

            // Create the user
            var user = new User
            {
                Email = cmdParams.Email,
                Password = PasswordUtils.CreatePasswordHash(cmdParams.Email, cmdParams.PlainTextPassword),

                JobSearches = new List<JobSearch>()
            };

            unitOfWork.Users.Add(user);
            unitOfWork.Commit();

            return user;
        }
    }
}
