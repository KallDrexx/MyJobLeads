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
    /// Queries for a user by its email
    /// </summary>
    public class UserByEmailQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected string _email;

        public UserByEmailQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the email of the user to retrieve
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserByEmailQuery WithEmail(string email)
        {
            _email = email;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLUserNotFoundException">Thrown when no user was found</exception>
        public User Execute()
        {
            var user = _unitOfWork.Users.Fetch().Where(x => x.Email == _email).SingleOrDefault();
            if (user == null)
                throw new MJLUserNotFoundException(MJLUserNotFoundException.SearchPropertyType.Email, _email);

            return user;
        }
    }
}
