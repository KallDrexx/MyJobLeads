using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.ViewModels.Milestones;
using MyJobLeads.DomainModel.ProcessParams.Milestones;
using MyJobLeads.DomainModel.ViewModels.Milestones;

namespace MyJobLeads.Infrastructure.ViewModelMappings
{
    public class MilestoneMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<EditMilestoneViewModel, SaveMilestoneParams>()
                .ForMember(dest => dest.OrganizationId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());

            Mapper.CreateMap<MilestoneDisplayViewModel, EditMilestoneViewModel>()
                .ForMember(dest => dest.AvailableMilestoneList, opt => opt.Ignore());
        }
    }
}