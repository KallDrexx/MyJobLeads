using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.DomainModel.ProcessParams.Admin;

namespace MyJobLeads.Areas.Admin.Models.Mappings
{
    public class OrganizationMappings : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<CreateOrganizationWithAdminViewModel, CreateOrgWithAdminUserParams>()
                .ForMember(dest => dest.RequestingUserId, opt => opt.Ignore());
        }
    }
}