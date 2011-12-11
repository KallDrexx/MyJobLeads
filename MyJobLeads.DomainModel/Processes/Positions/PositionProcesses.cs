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
using System.Diagnostics.Contracts;
using System.Data.Entity;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.DomainModel.Processes.Positions
{
    public class PositionProcesses 
        : IProcess<CreatePositionParams, PositionDisplayViewModel>,
          IProcess<EditPositionParams, PositionDisplayViewModel>,
          IProcess<GetPositionListForUserParams, PositionListViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<PositionAuthorizationParams, AuthorizationResultViewModel> _positionAuthProcess;
        protected IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> _companyAuthProcess;

        public PositionProcesses(MyJobLeadsDbContext context, 
                                IProcess<PositionAuthorizationParams, AuthorizationResultViewModel> positionAuthProcess,
                                IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> companyAuthProcess)
        {
            _context = context;
            _positionAuthProcess = positionAuthProcess;
            _companyAuthProcess = companyAuthProcess;
        }

        public PositionDisplayViewModel Execute(CreatePositionParams procParams)
        {
            // Make sure the user is authorized for the company
            var authParams = new CompanyQueryAuthorizationParams { CompanyId = procParams.CompanyId, RequestingUserId = procParams.RequestingUserId };
            if (!_companyAuthProcess.Execute(authParams).UserAuthorized)
                throw new UserNotAuthorizedForEntityException(typeof(Company), procParams.CompanyId, procParams.RequestingUserId);

            var company = _context.Companies.SingleOrDefault(x => x.Id == procParams.CompanyId);
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), procParams.CompanyId);

            var position = new Position
            {
                Title = procParams.Title,
                Notes = procParams.Notes,
                HasApplied = procParams.HasApplied,
                Company = company,
                LinkedInId = procParams.LinkedInId
            };

            _context.Positions.Add(position);
            _context.SaveChanges();

            return Mapper.Map<Position, PositionDisplayViewModel>(position);
        }

        public PositionDisplayViewModel Execute(EditPositionParams procParams)
        {
            // Verify the user is authorized for the position
            var authParams = new PositionAuthorizationParams { PositionId = procParams.Id, RequestingUserId = procParams.RequestingUserId };
            if (!_positionAuthProcess.Execute(authParams).UserAuthorized)
                throw new UserNotAuthorizedForEntityException(typeof(Position), procParams.Id, procParams.RequestingUserId);

            Position position = _context.Positions.SingleOrDefault(x => x.Id == procParams.Id);
            if (position == null)
                throw new MJLEntityNotFoundException(typeof(Position), procParams.Id);

            position.Title = procParams.Title;
            position.HasApplied = procParams.HasApplied;
            position.Notes = procParams.Notes;

            _context.SaveChanges();

            return Mapper.Map<Position, PositionDisplayViewModel>(position);
        }

        public PositionListViewModel Execute(GetPositionListForUserParams procParams)
        {
            var user = _context.Users.Where(x => x.Id == procParams.UserId).FirstOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.UserId);

            var positions = _context.Positions
                                    .Where(x => x.Company.JobSearch.Id == user.LastVisitedJobSearchId)
                                    .Include(x => x.Company)
                                    .ToList();

            var model = new PositionListViewModel { Positions = new List<PositionDisplayViewModel>() };

            foreach (var pos in positions)
                model.Positions.Add(Mapper.Map<Position, PositionDisplayViewModel>(pos));

            return model;
        }
    }
}
