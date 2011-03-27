using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Queries.Users
{
    /// <summary>
    /// Queries for a user by the specifies username
    /// </summary>
    public class UserByUsernameQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected string _username;

        public UserByUsernameQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the username for the user to search for
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserByUsernameQuery WithUsername(string username)
        {
            _username = username;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLUserNotFoundException">Thrown when no user exists with the specified username</exception>
        public User Execute()
        {
            return _unitOfWork.Users.Fetch().Where(x => x.Username == _username.Trim().ToLower()).SingleOrDefault();
        }
    }
}
