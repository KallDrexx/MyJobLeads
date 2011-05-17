using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.JobSearches
{
    /// <summary>
    /// Query that determines if a user has access to a jobsearch
    /// </summary>
    public class IsUserAuthorizedForJobSearchQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId, _searchId;

        public IsUserAuthorizedForJobSearchQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies id value of the user to check authorization for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForJobSearchQuery WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the job search to check authorization for
        /// </summary>
        /// <param name="jobSearchid"></param>
        /// <returns></returns>
        public IsUserAuthorizedForJobSearchQuery WithJobSearchId(int jobSearchid)
        {
            _searchId = jobSearchid;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns>Returns true if the user is authorized to access the job search</returns>
        public bool Execute()
        {
            // Check if the user is associated with the job search
            return _unitOfWork.JobSearches.Fetch().Any(x => x.Id == _searchId && x.User.Id == _userId);
        }
    }
}
