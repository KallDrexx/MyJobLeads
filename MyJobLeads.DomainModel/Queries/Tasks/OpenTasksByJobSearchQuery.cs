using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query that retrieves all open tasks for a job search
    /// </summary>
    public class OpenTasksByJobSearchQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _jobSearchId;

        public OpenTasksByJobSearchQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Specifies the id value of the job search to find open tasks with
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public OpenTasksByJobSearchQuery WithJobSearch(int jobSearchId)
        {
            _jobSearchId = jobSearchId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public IList<Task> Execute()
        {
            // Retrieve all tasks that are not completed that are associated with the specified job search
            return _unitOfWork.Tasks
                              .Fetch()
                              .Where(x => (x.Company.JobSearch.Id == _jobSearchId || x.Contact.Company.JobSearch.Id == _jobSearchId)
                                            && !x.Completed)
                              .OrderBy(x => x.TaskDate)
                              .ToList();
        }
    }
}
