using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.ViewModels.Positions;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.DomainModel.ProcessParams.Positions;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings
{
    public class PositionMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<Position, PositionDisplayViewModel>();

            Mapper.CreateMap<Position, EditPositionViewModel>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => Mapper.Map<Company, CompanySummaryViewModel>(src.Company)));

            Mapper.CreateMap<EditPositionViewModel, EditPositionParams>();
            Mapper.CreateMap<EditPositionViewModel, CreatePositionParams>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.Id));
        }
    }
}
