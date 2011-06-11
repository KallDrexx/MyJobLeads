using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Providers;

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
        protected IServiceFactory _serviceFactory;

        public UpdateJobSearchMetricsCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        virtual public void Execute(UpdateJobSearchMetricsCmdParams cmdParams)
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the job search 
            var jobSearch = new JobSearchByIdQuery(unitOfWork).WithJobSearchId(cmdParams.JobSearchId).Execute();

            // Rebuild Metrics
            jobSearch.Metrics.NumCompaniesCreated = unitOfWork.Companies.Fetch().Count(x => x.JobSearchID == cmdParams.JobSearchId);
            jobSearch.Metrics.NumContactsCreated = unitOfWork.Contacts.Fetch().Count(x => x.Company.JobSearchID == cmdParams.JobSearchId);
            jobSearch.Metrics.NumApplyTasksCreated = unitOfWork.Tasks.Fetch()
                                                                      .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                      .Where(x => x.Category == MJLConstants.ApplyToFirmTaskCategory)
                                                                      .Count();

            jobSearch.Metrics.NumApplyTasksCompleted = unitOfWork.Tasks.Fetch()
                                                                        .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                        .Where(x => x.Category == MJLConstants.ApplyToFirmTaskCategory)
                                                                        .Where(x => x.CompletionDate != null)
                                                                        .Count();

            jobSearch.Metrics.NumPhoneInterviewTasksCreated = unitOfWork.Tasks.Fetch()
                                                                               .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                               .Where(x => x.Category == MJLConstants.PhoneInterviewTaskCategory)
                                                                               .Count();

            jobSearch.Metrics.NumInPersonInterviewTasksCreated = unitOfWork.Tasks.Fetch()
                                                                                  .Where(x => x.Company.JobSearchID == cmdParams.JobSearchId)
                                                                                  .Where(x => x.Category == MJLConstants.InPersonInterviewTaskCategory)
                                                                                  .Count();

            // Update the entity
            unitOfWork.Commit();
        }
    }
}
