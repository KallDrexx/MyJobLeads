using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings
{
    public class MilestoneConfigMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<MilestoneConfig, MyJobLeads.DomainModel.ViewModels.Milestones.MilestoneDisplayListViewModel.MilestoneDisplayViewModel>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));
        }
    }
}
