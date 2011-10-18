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

            // Place the milestone in it's specified spot
            var startingMilestone = milestone.Organization.MilestoneConfigurations.Where(x => x.IsStartingMilestone).FirstOrDefault();
            if (startingMilestone == null)
            {
                // There should be no milestones since none is starting
                milestone.IsStartingMilestone = true;
            }

            else
            {
                // First loop through all the milestones and unlink the current milestone
                MilestoneConfig currentMilestone = startingMilestone;
                while (currentMilestone != null)
                {
                    if (currentMilestone.NextMilestone == milestone)
                    {
                        currentMilestone.NextMilestone = milestone.NextMilestone;
                        break;
                    }

                    currentMilestone = currentMilestone.NextMilestone;
                }

                // If the previous milestone id is zero, set it as the starting milestone
                if (procParams.PreviousMilestoneId == 0)
                {
                    milestone.IsStartingMilestone = true;
                    milestone.NextMilestone = startingMilestone;
                    startingMilestone.IsStartingMilestone = false;
                }

                else
                {
                    // Do a second loop to add the milestone back into it's correct position
                    currentMilestone = startingMilestone;
                    while (currentMilestone != null)
                    {
                        if (currentMilestone.Id == procParams.PreviousMilestoneId)
                        {
                            milestone.NextMilestone = currentMilestone.NextMilestone;
                            currentMilestone.NextMilestone = milestone;
                            break;
                        }

                        currentMilestone = currentMilestone.NextMilestone;
                    }
                }
            }
                
            _context.SaveChanges();
            return new MilestoneIdViewModel { Id = milestone.Id };
        }
    }
}
