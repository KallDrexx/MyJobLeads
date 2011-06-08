using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Entities.Metrics;

namespace MyJobLeads.DomainModel.Entities
{
    public class JobSearch
    {
        public JobSearch() { Metrics = new JobSearchMetrics(); }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public JobSearchMetrics Metrics { get; set; }

        public virtual User User { get; set; }
        public virtual int? UserId { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<JobSearchHistory> History { get; set; }
    }
}
