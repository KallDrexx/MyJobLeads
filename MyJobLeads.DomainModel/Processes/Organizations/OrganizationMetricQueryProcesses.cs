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

namespace MyJobLeads.DomainModel.Processes.Organizations
{
    public class OrganizationMetricQueryProcesses : IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public OrganizationMetricQueryProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of members of an organization and their job search metrics for users 
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public OrganizationMemberStatisticsViewModel Execute(OrganizationMemberStatisticsParams procParams)
        {
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
