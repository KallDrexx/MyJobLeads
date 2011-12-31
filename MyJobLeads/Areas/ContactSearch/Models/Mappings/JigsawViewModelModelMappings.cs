using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw;

namespace MyJobLeads.Areas.ContactSearch.Models.Mappings
{
    public class JigsawViewModelModelMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<JigsawSearchParametersViewModel, JigsawContactSearchParams>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Levels, opt => opt.MapFrom(src => src.Levels))
                .ForMember(dest => dest.RequestedPageNum, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());

            Mapper.CreateMap<AddJigsawContactViewModel, AddJigsawContactToJobSearchParams>()
                .ForMember(dest => dest.JigsawCompanyId, opt => opt.MapFrom(src => src.JigsawCompanyId))
                .ForMember(dest => dest.JigsawContactId, opt => opt.MapFrom(src => src.JigsawContactId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreateCompanyFromJigsaw, opt => opt.MapFrom(src => src.CreateNewCompany))
                .ForMember(dest => dest.ExistingCompanyId, opt => opt.MapFrom(src => src.SelectedCompanyId))
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());
        }
    }
}