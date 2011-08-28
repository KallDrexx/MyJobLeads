using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Positions;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings
{
    public class PositionMapping : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<Position, PositionDisplayViewModel>();
        }
    }
}
