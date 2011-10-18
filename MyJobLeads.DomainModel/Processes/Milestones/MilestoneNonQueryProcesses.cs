using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Milestones;
using MyJobLeads.DomainModel.ProcessParams.Milestones;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities.Configuration;
using AutoMapper;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Processes.Milestones
{
    public class MilestoneNonQueryProcesses : IProcess<SaveMilestoneParams, MilestoneIdViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public MilestoneNonQueryProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public MilestoneIdViewModel Execute(SaveMilestoneParams procParams)
        {
            MilestoneConfig milestone;

            // Determine if we are creating a new milestone or editing an existing one
            if (procParams.Id == 0)
            {
                var org = _context.Organizations.Where(x => x.Id == procParams.OrganizationId).FirstOrDefault();
                if (!org.Members.Any(x => x.Id == procParams.RequestingUserId && x.IsOrganizationAdmin))
                    throw new UserNotAuthorizedForEntityException(typeof(Organization), procParams.OrganizationId, procParams.RequestingUserId);

                milestone = Mapper.Map<SaveMilestoneParams, MilestoneConfig>(procParams);
                milestone.Organization = org;
                _context.MilestoneConfigs.Add(milestone);
            }
            
            else
            {
                milestone = _context.MilestoneConfigs
                                    .Where(x => x.Id == procParams.Id)
                                    .Include(x => x.Organization)
                                    .SingleOrDefault();

                // Make sure the user is authorized to edit the milestone
                if (milestone.Organization != null)
                {
                    if (!milestone.Organization.Members.Any(x => x.Id == procParams.RequestingUserId && x.IsOrganizationAdmin))
                        throw new UserNotAuthorizedForEntityException(typeof(Organization), milestone.Organization.Id, procParams.RequestingUserId);
                }

                milestone = Mapper.Map<SaveMilestoneParams, MilestoneConfig>(procParams, milestone);
            }
                
            _context.SaveChanges();
            return new MilestoneIdViewModel { Id = milestone.Id };
        }
    }
}
