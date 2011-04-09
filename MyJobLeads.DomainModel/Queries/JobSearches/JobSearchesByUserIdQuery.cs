using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.JobSearches
{
    /// <summary>
    /// Query to retrieve all job searches that belong to a specific user
    /// </summary>
    public class JobSearchesByUserIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId;

        public JobSearchesByUserIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the user to retrieve job searches for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JobSearchesByUserIdQuery WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual IList<JobSearch> Execute()
        {
            // Retrieve all job searches for the user
            return _unitOfWork.JobSearches.Fetch().Where(x => x.User.Id == _userId).ToList();
        }
    }
}
