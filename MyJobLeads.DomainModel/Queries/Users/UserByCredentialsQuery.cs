using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.DomainModel.Queries.Users
{
    /// <summary>
    /// Query that retrieves a user by the specified user credentials
    /// </summary>
    public class UserByCredentialsQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected string _email;
        protected string _password;

        public UserByCredentialsQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the email address of the user to query for
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserByCredentialsQuery WithEmail(string username)
        {
            _email = username;
            return this;
        }

        /// <summary>
        /// Specifies the password for the user to search for
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserByCredentialsQuery WithPassword(string password)
        {
            _password = password;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public User Execute()
        {
            // First return the user with the matching username
            var user = _unitOfWork.Users
                                  .Fetch()
                                  .Where(x => x.Email == _email)
                                  .SingleOrDefault();

            // If no user was found, return null
            if (user == null)
                return null;

            // Check if the password is correct
            if (PasswordUtils.CheckPasswordHash(_email, _password, user.Password))
                return user;
            else
                return null;
        }
    }
}
