using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.Areas.CompanySearch.Models.Jigsaw;
using MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw;

namespace MyJobLeads.Areas.CompanySearch.Models.Mappings
{
    public class JigsawSearchMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<CompanySearchQueryViewModel, JigsawCompanySearchParams>()
                .ForMember(dest => dest.RequestedPageNum, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());
        }
    }
}