using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Commands.JobSearches
{
    public struct StartNextJobSearchMilestoneCommandParams
    {
        public int JobSearchId { get; set; }
    }

    /// <summary>
    /// Command class that changes a job search's current milestone to the next one if the current milestone is completed
    /// </summary>
    public class StartNextJobSearchMilestoneCommand
    {
        protected IServiceFactory _serviceFactory;

        public StartNextJobSearchMilestoneCommand(IServiceFactory iServiceFactory)
        {
            _serviceFactory = iServiceFactory;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="cmdParams"></param>
        public void Execute(StartNextJobSearchMilestoneCommandParams cmdParams)
        {
            // Retrieve the specified job search
            var search = _serviceFactory.GetService<JobSearchByIdQuery>().WithJobSearchId(cmdParams.JobSearchId).Execute();
            if (search == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), cmdParams.JobSearchId);

            // If the job search has no current milestone, do nothing
            if (search.CurrentMilestoneId == null)
                return;

            // Determine if the current milestone has been completed
            var progress = new JobSearchMilestoneProgress(search);
            if (progress.TotalProgress == 1)
            {
                search.CurrentMilestone = search.CurrentMilestone.NextMilestone;
                _serviceFactory.GetService<IUnitOfWork>().Commit();
            }
        }
    }
}
