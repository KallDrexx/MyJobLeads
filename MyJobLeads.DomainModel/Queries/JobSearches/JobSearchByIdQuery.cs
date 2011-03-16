using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Queries.JobSearches
{
    /// <summary>
    /// Queries for a job search by id value
    /// </summary>
    public class JobSearchByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _jobSearchId;

        public JobSearchByIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the job search to retrieve
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public JobSearchByIdQuery WithJobSearchId(int jobSearchId)
        {
            _jobSearchId = jobSearchId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the requested job search does not exist</exception>
        public JobSearch Execute()
        {
            // Attempt to retrieve the job search
            var jobSearch = _unitOfWork.JobSearches.Fetch().Where(x => x.Id == _jobSearchId).SingleOrDefault();
            if (jobSearch == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), _jobSearchId);

            return jobSearch;
        }
    }
}
