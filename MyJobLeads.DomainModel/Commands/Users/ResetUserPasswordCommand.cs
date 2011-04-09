using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Commands.Users
{
    /// <summary>
    /// Command class that resets the user's password
    /// </summary>
    public class ResetUserPasswordCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId;
        public EmailUtils EmailProvider { get; protected set; }

        public ResetUserPasswordCommand(IUnitOfWork unitOfWork) : this(unitOfWork, new EmailUtils()) { }

        public ResetUserPasswordCommand(IUnitOfWork unitOfWork, EmailUtils emailUtils)
        {
            _unitOfWork = unitOfWork;
            EmailProvider = emailUtils;
        }

        /// <summary>
        /// Specifies the id value of the user whose password to reset
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResetUserPasswordCommand WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>Returns the generated password for the user</returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified user is not found</exception>
        public virtual string Execute()
        {
            // Retrieve the user
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Generate a new password for the user
            string newPassword = PasswordUtils.GenerateRandomPassword();
            user.Password = PasswordUtils.CreatePasswordHash(user.Email, newPassword);

            _unitOfWork.Commit();

            // Send out an email
            EmailProvider.Send(user.Email, "Password Recovery", "Your new MyJobLeads password is: " + newPassword);

            return newPassword;
        }
    }
}
