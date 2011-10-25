using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Entities.Metrics;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.DomainModel.Entities
{
    public class JobSearch
    {
        public JobSearch() 
        { 
            Metrics = new JobSearchMetrics();
            Companies = new List<Company>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public JobSearchMetrics Metrics { get; set; }
        public string HiddenCompanyStatuses { get; set; } // semi-colon delimeted string
        public bool MilestonesCompleted { get; set; }

        public virtual User User { get; set; }
        public virtual int? UserId { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<JobSearchHistory> History { get; set; }

        public virtual MilestoneConfig CurrentMilestone { get; set; }
        public virtual int? CurrentMilestoneId { get; set; }
    }
}
