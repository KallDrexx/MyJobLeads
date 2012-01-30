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

            Mapper.CreateMap<SyncCompanyViewModel, MergeJigsawCompanyParams>()
                .ForMember(dest => dest.JigsawCompanyId, opt => opt.MapFrom(src => src.JigsawId))
                .ForMember(dest => dest.MyLeadsCompayId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.MergeAddress, opt => opt.MapFrom(src => src.ImportAddress))
                .ForMember(dest => dest.MergeIndustry, opt => opt.MapFrom(src => src.ImportIndustry))
                .ForMember(dest => dest.MergeName, opt => opt.MapFrom(src => src.ImportName))
                .ForMember(dest => dest.MergePhone, opt => opt.MapFrom(src => src.ImportPhone))
                .ForMember(dest => dest.MergeEmployeeCount, opt => opt.Ignore())
                .ForMember(dest => dest.MergeOwnership, opt => opt.Ignore())
                .ForMember(dest => dest.MergeRevenue, opt => opt.Ignore())
                .ForMember(dest => dest.MergeWebsite, opt => opt.Ignore())
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());
        }
    }
}