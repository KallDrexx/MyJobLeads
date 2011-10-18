using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.ViewModels.Milestones;
using MyJobLeads.DomainModel.ProcessParams.Milestones;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings
{
    public class MilestoneMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<MilestoneConfig, MilestoneIdViewModel>();
            Mapper.CreateMap<SaveMilestoneParams, MilestoneConfig>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsStartingMilestone, opt => opt.Ignore())
                .ForMember(dest => dest.NextMilestone, opt => opt.Ignore())
                .ForMember(dest => dest.NextMilestoneId, opt => opt.Ignore())
                .ForMember(dest => dest.OrganizationId, opt => opt.Ignore());
        }
    }
}
