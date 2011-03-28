using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.DomainModel.Commands.Users
{
    /// <summary>
    /// Command class that edits the properties of a user
    /// </summary>
    public class EditUserCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected string _newEmail, _newPassword;
        protected int _userId;

        public EditUserCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the user to edit
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditUserCommand WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the email for the user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public EditUserCommand SetEmail(string email)
        {
            _newEmail = email;
            return this;
        }

        /// <summary>
        /// Specifies the new password for the user
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public EditUserCommand SetPassword(string password)
        {
            _newPassword = password;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified user isn't found</exception>
        public User Execute()
        {
            // Retrieve the user
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Edit the properties
            if (_newEmail != null) { user.Email = _newEmail; }
            if (_newPassword != null) { user.Password = PasswordUtils.CreatePasswordHash(user.Username, _newPassword); }

            // Save
            _unitOfWork.Commit();
            return user;
        }
    }
}
