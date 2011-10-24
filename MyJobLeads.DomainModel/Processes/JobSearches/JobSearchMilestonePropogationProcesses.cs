using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.JobSearches;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Processes.JobSearches
{
    public class JobSearchMilestonePropogationProcesses : IProcess<JobSearchMilestonePropogationParams, GeneralSuccessResultViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public JobSearchMilestonePropogationProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Propogate's a starting milestoe to the specified job search if required
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(JobSearchMilestonePropogationParams procParams)
        {
            var search = _context.JobSearches
                                 .Where(x => x.Id == procParams.JobSearchId)
                                 .Include(x => x.User.Organization)
                                 .FirstOrDefault();

            if (search == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), procParams.JobSearchId);

            if (!search.MilestonesCompleted && search.CurrentMilestone == null)
            {
                var startingMilestone = search.User.Organization.MilestoneConfigurations.Where(x => x.IsStartingMilestone).SingleOrDefault();
                search.CurrentMilestone = startingMilestone;
                _context.SaveChanges();
            }

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }
    }
}
