using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.EntityMapping.Mappings.CustomConverters;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings
{
    public class OrganizationMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<IList<User>, OrganizationMemberStatisticsViewModel>()
                  .ConvertUsing(new OrganizationMemberStatisticsViewModelConverter());
        }
    }
}
