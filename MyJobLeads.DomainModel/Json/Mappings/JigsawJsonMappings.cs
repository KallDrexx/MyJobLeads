using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.Json.Jigsaw;

namespace MyJobLeads.DomainModel.Json.Mappings
{
    public class JigsawJsonMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<ContactDetailsJson, ExternalContactSearchResultsViewModel.ContactResultViewModel>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.HasAccess, opt => opt.MapFrom(src => src.Owned))
                .ForMember(dest => dest.Headline, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.PublicUrl, opt => opt.Ignore());

            Mapper.CreateMap<ContactSearchResponseJson, ExternalContactSearchResultsViewModel>()
                .ForMember(dest => dest.TotalResultsCount, opt => opt.MapFrom(src => src.TotalHits))
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(dest => dest.PageSize, opt => opt.Ignore())
                .ForMember(dest => dest.DisplayedPageNumber, opt => opt.Ignore());
        }
    }
}
