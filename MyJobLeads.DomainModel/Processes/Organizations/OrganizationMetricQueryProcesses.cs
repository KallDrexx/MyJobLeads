using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.Entities.EF;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Processes.Organizations
{
    public class OrganizationMetricQueryProcesses : IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> _orgAdminAuthProcess;

        public OrganizationMetricQueryProcesses(MyJobLeadsDbContext context, IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel> orgAdminAuthProc)
        {
            _context = context;
            _orgAdminAuthProcess = orgAdminAuthProc;
        }

        /// <summary>
        /// Retrieves a list of members of an organization and their job search metrics for users 
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public OrganizationMemberStatisticsViewModel Execute(OrganizationMemberStatisticsParams procParams)
        {
            var authResult = _orgAdminAuthProcess.Execute(new OrganizationAdminAuthorizationParams
            {
                OrganizationId = procParams.OrganizationId,
                UserId = procParams.RequestingUserId
            });
            if (!authResult.UserAuthorized)
                throw new UserNotAuthorizedForEntityException(typeof(Organization), procParams.OrganizationId, procParams.RequestingUserId);

            var members = _context.Users
                                  .Where(x => x.Organization.Id == procParams.OrganizationId)
                                  .Include(x => x.LastVisitedJobSearch)
                                  .ToList();

            var result = Mapper.Map<IList<User>, OrganizationMemberStatisticsViewModel>(members);
            result.OrganizationId = procParams.OrganizationId;
            return result;
        }
    }
}
