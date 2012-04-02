using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.DomainModel.Entities.FillPerfect;

namespace MyJobLeads.DomainModel.ViewModels.Mail.Mappings
{
    public class FillPerfectMailMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<FpOrgPilotUsedLicense, FillPerfectPilotStudentLicenceEmailViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.FpKey, opt => opt.MapFrom(src => src.Organization.FillPerfectPilotKey));
        }
    }
}
