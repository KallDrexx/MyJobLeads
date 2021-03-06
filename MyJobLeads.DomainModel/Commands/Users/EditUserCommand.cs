﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Utilities;
using FluentValidation.Results; 

namespace MyJobLeads.DomainModel.Commands.Users
{
    /// <summary>
    /// Command class that edits the properties of a user
    /// </summary>
    public class EditUserCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected string _newPassword, _oldPassword, _newName;
        protected int _userId, _lastJobSearchId;
        protected bool _updateLastJobSearch;

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
        /// Specifies the id value of the job search the user last visited
        /// </summary>
        /// <param name="lastVisitedJobSearchId"></param>
        /// <returns></returns>
        public EditUserCommand SetLastVisitedJobSearchId(int lastVisitedJobSearchId)
        {
            _lastJobSearchId = lastVisitedJobSearchId;
            _updateLastJobSearch = true;
            return this;
        }

        /// <summary>
        /// Specifies the current password for the user
        /// </summary>
        /// <param name="currentPassword"></param>
        /// <returns></returns>
        public EditUserCommand WithExistingPassword(string currentPassword)
        {
            _oldPassword = currentPassword;
            return this;
        }

        /// <summary>
        /// Specifies the new name for the user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EditUserCommand SetFullName(string name)
        {
            _newName = name;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified user isn't found</exception>
        public virtual User Execute()
        {
            // Retrieve the user
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Only check the current password if the user is only changing the last visited job search
            if (!string.IsNullOrWhiteSpace(_newPassword) || !string.IsNullOrWhiteSpace(_newName))
            {
                // Verify the user's current password is correct
                if (new UserByCredentialsQuery(_unitOfWork).WithEmail(user.Email).WithPassword(_oldPassword).Execute() == null)
                    throw new MJLIncorrectPasswordException(_userId);
            }

            // Edit the properties
            if (!string.IsNullOrWhiteSpace(_newPassword))
                user.Password = PasswordUtils.CreatePasswordHash(user.Email, _newPassword);

            if (_newName != null) { user.FullName = _newName.Trim(); }
            if (_updateLastJobSearch) { user.LastVisitedJobSearchId = _lastJobSearchId; }

            // Save
            _unitOfWork.Commit();
            return user;
        }
    }
}
