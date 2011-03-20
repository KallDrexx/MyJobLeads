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
        protected string _username;
        protected string _password;

        public UserByCredentialsQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the user name of the user to query for
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserByCredentialsQuery WithUsername(string username)
        {
            _username = username;
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
            // Make the supplised username lower case and trimmed
            _username = _username.Trim().ToLower();

            // Return the user with the matching username
            var user = _unitOfWork.Users
                                  .Fetch()
                                  .Where(x => x.Username == _username)
                                  .SingleOrDefault();

            // If no user was found, return null
            if (user == null)
                return null;

            // Check if the password is correct
            if (PasswordUtils.CheckPasswordHash(_username, _password, user.Password))
                return user;
            else
                return null;
        }
    }
}
