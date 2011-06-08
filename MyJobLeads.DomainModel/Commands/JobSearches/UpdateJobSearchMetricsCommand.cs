using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;

namespace MyJobLeads.DomainModel.Commands.JobSearches
{
    /// <summary>
    /// Command that updates the metrics for a job search
    /// </summary>
    public class UpdateJobSearchMetricsCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected int _jobSearchId;

        public UpdateJobSearchMetricsCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the job search to update metrics for
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public UpdateJobSearchMetricsCommand WithJobSearchId(int jobSearchId)
        {
            _jobSearchId = jobSearchId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        virtual public void Execute()
        {
            // Retrieve the job search 
            var jobSearch = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(_jobSearchId).Execute();

            // Rebuild Metrics
            jobSearch.Metrics.NumCompaniesCreated = _unitOfWork.Companies.Fetch().Count(x => x.JobSearchID == _jobSearchId);
            jobSearch.Metrics.NumContactsCreated = _unitOfWork.Contacts.Fetch().Count(x => x.Company.JobSearchID == _jobSearchId);
            jobSearch.Metrics.NumApplyTasksCreated = _unitOfWork.Tasks.Fetch()
                                                                      .Where(x => x.Company.JobSearchID == _jobSearchId)
                                                                      .Where(x => x.Category == MJLConstants.ApplyToFirmTaskCategory)
                                                                      .Count();

            jobSearch.Metrics.NumApplyTasksCompleted = _unitOfWork.Tasks.Fetch()
                                                                        .Where(x => x.Company.JobSearchID == _jobSearchId)
                                                                        .Where(x => x.Category == MJLConstants.ApplyToFirmTaskCategory)
                                                                        .Where(x => x.CompletionDate != null)
                                                                        .Count();

            jobSearch.Metrics.NumPhoneInterviewTasksCreated = _unitOfWork.Tasks.Fetch()
                                                                               .Where(x => x.Company.JobSearchID == _jobSearchId)
                                                                               .Where(x => x.Category == MJLConstants.PhoneInterviewTaskCategory)
                                                                               .Count();

            jobSearch.Metrics.NumInPersonInterviewTasksCreated = _unitOfWork.Tasks.Fetch()
                                                                                  .Where(x => x.Company.JobSearchID == _jobSearchId)
                                                                                  .Where(x => x.Category == MJLConstants.InPersonInterviewTaskCategory)
                                                                                  .Count();

            // Update the entity
            _unitOfWork.Commit();
        }
    }
}
