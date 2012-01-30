using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.Areas.CompanySearch.Models.Jigsaw;
using MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.CompanySearching;

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
                .ForMember(dest => dest.MergeEmployeeCount, opt => opt.MapFrom(src => src.ImportEmployeeCount))
                .ForMember(dest => dest.MergeOwnership, opt => opt.MapFrom(src => src.ImportOwnership))
                .ForMember(dest => dest.MergeRevenue, opt => opt.MapFrom(src => src.ImportRevenue))
                .ForMember(dest => dest.MergeWebsite, opt => opt.MapFrom(src => src.ImportWebsite))
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());

            Mapper.CreateMap<Company, SyncCompanyViewModel>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.InternalIndustry, opt => opt.MapFrom(src => src.Industry))
                .ForMember(dest => dest.InternalName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.InternalPhone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.InternalAddress, opt => opt.MapFrom(src => string.Format("{0}, {1} {2}", src.City, src.State, src.Zip)));

            Mapper.CreateMap<ExternalCompanyDetailsViewModel, SyncCompanyViewModel>()
                .ForMember(dest => dest.JigsawId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.JigsawAddress, opt => opt.MapFrom(src => string.Format("{0}, {1} {2}", src.City, src.State, src.Zip)))
                .ForMember(dest => dest.JigsawEmployeeCount, opt => opt.MapFrom(src => src.EmployeeCount))
                .ForMember(dest => dest.JigsawName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.JigsawOwnership, opt => opt.MapFrom(src => src.Ownership))
                .ForMember(dest => dest.JigsawPhone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.JigsawRevenue, opt => opt.MapFrom(src => src.Revenue))
                .ForMember(dest => dest.JigsawWebsite, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.LastUpdatedOnJigaw, opt => opt.MapFrom(src => src.LastUpdated))
                .ForMember(dest => dest.JigsawIndustry, opt => opt.MapFrom(src => src.SingleIndustry));
        }
    }
}