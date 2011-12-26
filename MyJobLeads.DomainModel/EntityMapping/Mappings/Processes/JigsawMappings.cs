using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Json.Jigsaw;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings.Processes
{
    public class JigsawMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<ContactSearchResponseJson, ExternalContactSearchResultsViewModel>()
                .ForMember(dest => dest.TotalResultsCount, opt => opt.MapFrom(src => src.TotalHits))
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(dest => dest.DisplayedPageNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PageSize, opt => opt.Ignore());

            Mapper.CreateMap<ContactDetailsResponseJson, ExternalContactSearchResultsViewModel.ContactResultViewModel>()
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Headline, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PublicUrl, opt => opt.MapFrom(src => src.ContactUrl))
                .ForMember(dest => dest.HasAccess, opt => opt.MapFrom(src => src.Owned))
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}
