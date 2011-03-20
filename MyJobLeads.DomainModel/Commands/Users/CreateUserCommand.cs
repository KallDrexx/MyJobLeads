﻿using System;
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
        protected string _email, _password, _displayName;

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
        /// Specifies the display name for the new user
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateUserCommand SetDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        public User Execute()
        {
            // Convert the email to lower case and trim it
            _email = _email.Trim().ToLower();

            // Create the user
            var user = new User
            {
                Email = _email,
                DisplayName = _displayName,
                Password = PasswordUtils.CreatePasswordHash(_email, _password)
            };

            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            return user;
        }
    }
}
