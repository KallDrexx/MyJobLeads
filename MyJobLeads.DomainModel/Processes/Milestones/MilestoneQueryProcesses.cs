using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Milestones;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.Entities.EF;
using AutoMapper;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Milestones;

namespace MyJobLeads.DomainModel.Processes.Milestones
{
    public class MilestoneQueryProcesses : IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel>,
                                           IProcess<MilestoneIdParams, MilestoneDisplayViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public MilestoneQueryProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of milestones for the user's organization
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public MilestoneDisplayListViewModel Execute(ByOrganizationIdParams procParams)
        {
            if (!_context.Users.Any(x => x.Id == procParams.RequestingUserId && x.IsOrganizationAdmin))
                throw new UserNotAuthorizedForEntityException(typeof(Organization), procParams.OrganizationId, procParams.RequestingUserId);

            var milestones = _context.Users
                                     .Where(x => x.Id == procParams.RequestingUserId)
                                     .SelectMany(x => x.Organization.MilestoneConfigurations)
                                     .ToList();

            var model = new MilestoneDisplayListViewModel();
            foreach (var milestone in milestones)
                model.Milestones.Add(Mapper.Map<MilestoneConfig, MilestoneDisplayListViewModel.MilestoneDisplayViewModel>(milestone));

            return model;
        }

        /// <summary>
        /// Retrives the data for a specific params
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public MilestoneDisplayViewModel Execute(MilestoneIdParams procParams)
        {
            var milestone = _context.MilestoneConfigs.Where(x => x.Id == procParams.MilestoneId).SingleOrDefault();
            if (milestone == null)
                throw new MJLEntityNotFoundException(typeof(MilestoneConfig), procParams.MilestoneId);

            var user = _context.Users.Where(x => x.Id == procParams.RequestingUserId).SingleOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            if (!user.IsOrganizationAdmin || user.Organization != milestone.Organization)
                throw new UserNotAuthorizedForEntityException(typeof(MilestoneConfig), procParams.MilestoneId, procParams.RequestingUserId);
            
            // Form the view model
            var viewModel = Mapper.Map<MilestoneConfig, MilestoneDisplayViewModel>(milestone);
            if (!milestone.IsStartingMilestone)
            {
                var prevMilestone = _context.MilestoneConfigs.Where(x => x.NextMilestone.Id == milestone.Id).Single();
                viewModel.PreviousMilestoneId = prevMilestone.Id;
                viewModel.PreviousMilestoneName = prevMilestone.Title;
            }

            return viewModel;
        }
    }
}
