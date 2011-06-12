using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads.DomainModel.Queries.MilestoneConfigs
{
    public class StartingMilestoneQuery
    {
        protected IServiceFactory _serviceFactory;

        public StartingMilestoneQuery(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual MilestoneConfig Execute()
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            return unitOfWork.MilestoneConfigs.Fetch().FirstOrDefault(x => x.IsStartingMilestone);
        }
    }
}
