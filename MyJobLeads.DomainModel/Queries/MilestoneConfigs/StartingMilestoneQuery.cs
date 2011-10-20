using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;

namespace MyJobLeads.DomainModel.Queries.MilestoneConfigs
{
    public struct StartingMilestoneQueryParams
    {
        public int? OrganizationId { get; set; }
    }

    public class StartingMilestoneQuery
    {
        protected MyJobLeadsDbContext _context;

        public StartingMilestoneQuery(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual MilestoneConfig Execute(StartingMilestoneQueryParams cmdParams)
        {
            if (cmdParams.OrganizationId == null)
                return _context.MilestoneConfigs.FirstOrDefault(x => x.IsStartingMilestone);

            return _context.MilestoneConfigs.FirstOrDefault(x => x.IsStartingMilestone && x.Organization.Id == cmdParams.OrganizationId);
        }
    }
}
