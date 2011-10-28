using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.Metrics;

namespace MyJobLeads.DomainModel.ViewModels.Organizations
{
    public class OrganizationMemberStatisticsViewModel
    {
        public int OrganizationId { get; set; }
        public IList<MemberStatistic> MemberStats { get; set; }

        public class MemberStatistic
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public JobSearchMetrics Metrics { get; set; }
        }
    }
}
