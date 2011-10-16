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

namespace MyJobLeads.DomainModel.Processes.Milestones
{
    public class MilestoneQueryProcesses : IProcess<ByOrganizationIdParams, MilestoneDisplayListViewModel>
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
    }
}
