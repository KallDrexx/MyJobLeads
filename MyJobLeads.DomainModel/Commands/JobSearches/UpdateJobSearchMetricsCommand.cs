using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;

namespace MyJobLeads.DomainModel.Commands.JobSearches
{
    public struct UpdateJobSearchMetricsCmdParams
    {
        /// <summary>
        /// Specifies the id value of the job search to update
        /// </summary>
        public int JobSearchId { get; set; }

        /// <summary>
        /// Specifies the id value of the user updating the job search metrics
        /// </summary>
        public int RequestingUserId { get; set; }
    }

    /// <summary>
    /// Command that updates the metrics for a job search
    /// </summary>
    public class UpdateJobSearchMetricsCommand
    {
        protected IUnitOfWork _unitOfWork;

        public UpdateJobSearchMetricsCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        virtual public void Execute(UpdateJobSearchMetricsCmdParams cmdParams)
        {
            // Retrieve the job search 
            var jobSearch = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(cmdParams.JobSearchId).Execute();

            // Rebuild Metrics
            jobSearch.Metrics.NumCompaniesCreated = _unitOfWork.Companies.Fetch().Count(x => x.JobSearchID == cmdParams.JobSearchId);
            jobSearch.Metrics.NumContactsCreated = _unitOfWork.Contacts.Fetch().Count(x => x.Company.JobSearchID == cmdParams.JobSearchId);
            jobSearch.Metrics.NumApplyTasksCreated = _unitOfWork.Tasks.Fetch()
                                                                      .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                      .Where(x => x.Category == MJLConstants.ApplyToFirmTaskCategory)
                                                                      .Count();

            jobSearch.Metrics.NumApplyTasksCompleted = _unitOfWork.Tasks.Fetch()
                                                                        .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                        .Where(x => x.Category == MJLConstants.ApplyToFirmTaskCategory)
                                                                        .Where(x => x.CompletionDate != null)
                                                                        .Count();

            jobSearch.Metrics.NumPhoneInterviewTasksCreated = _unitOfWork.Tasks.Fetch()
                                                                               .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                               .Where(x => x.Category == MJLConstants.PhoneInterviewTaskCategory)
                                                                               .Count();

            jobSearch.Metrics.NumInPersonInterviewTasksCreated = _unitOfWork.Tasks.Fetch()
                                                                                  .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                                  .Where(x => x.Category == MJLConstants.InPersonInterviewTaskCategory)
                                                                                  .Count();

            // Update the entity
            _unitOfWork.Commit();
        }
    }
}
