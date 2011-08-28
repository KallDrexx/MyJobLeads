using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.DomainModel.Entities.Extensions;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings
{
    public class CompanyMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<Company, CompanySummaryViewModel>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.LocationString()));
        }
    }
}
