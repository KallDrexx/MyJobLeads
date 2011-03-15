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
    /// Retrieves a user by it's id value
    /// </summary>
    public class UserByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId;

        public UserByIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the user to retrieve
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserByIdQuery WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when no user exists with the specified id value</exception>
        public User Execute()
        {
            // Attempt to retrieve the user
            var user = _unitOfWork.Users.Fetch().Where(x => x.Id == _userId).SingleOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            return user;
        }
    }
}
