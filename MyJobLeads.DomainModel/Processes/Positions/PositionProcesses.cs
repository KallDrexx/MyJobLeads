using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities;
using AutoMapper;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Processes.Positions
{
    public class PositionProcesses 
        : IProcess<CreatePositionParams, PositionDisplayViewModel>,
          IProcess<EditPositionParams, PositionDisplayViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public PositionProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public PositionDisplayViewModel Execute(CreatePositionParams procParams)
        {
            var company = _context.Companies.SingleOrDefault(x => x.Id == procParams.CompanyId);
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), procParams.CompanyId);

            var position = new Position
            {
                Title = procParams.Title,
                Notes = procParams.Notes,
                HasApplied = procParams.HasApplied,
                Company = company
            };

            _context.Positions.Add(position);
            _context.SaveChanges();

            return Mapper.Map<Position, PositionDisplayViewModel>(position);
        }

        public PositionDisplayViewModel Execute(EditPositionParams procParams)
        {
            Position position = _context.Positions.SingleOrDefault(x => x.Id == procParams.Id);
            if (position == null)
                throw new MJLEntityNotFoundException(typeof(Position), procParams.Id);

            position.Title = procParams.Title;
            position.HasApplied = procParams.HasApplied;
            position.Notes = procParams.Notes;

            _context.SaveChanges();

            return Mapper.Map<Position, PositionDisplayViewModel>(position);
        }
    }
}
