using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.DomainModel.Queries.JobSearches
{
    /// <summary>
    /// Query for retrieving user metrics for a job search
    /// </summary>
    public class JobSearchUserMetricsQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _jobsearchId;

        public JobSearchUserMetricsQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the job search 
        /// </summary>
        /// <param name="jobsearchId"></param>
        /// <returns></returns>
        public JobSearchUserMetricsQuery WithJobSearchId(int jobsearchId)
        {
            _jobsearchId = jobsearchId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public ViewModels.JobSearchUserMetrics Execute()
        {
            var data = new JobSearchUserMetrics();

            // Retrieve the metrics
            data.NumTasks = _unitOfWork.Tasks.Fetch().Where(x => x.Company.JobSearchID == _jobsearchId).Count();
            data.NumTasksCompleted = _unitOfWork.Tasks.Fetch().Where(x => x.Company.JobSearchID == _jobsearchId && x.CompletionDate != null).Count();
            data.NumCompanies = _unitOfWork.Companies.Fetch().Where(x => x.JobSearchID == _jobsearchId).Count();
            data.NumContacts = _unitOfWork.Contacts.Fetch().Where(x => x.Company.JobSearchID == _jobsearchId).Count();

            return data;
        }
    }
}
