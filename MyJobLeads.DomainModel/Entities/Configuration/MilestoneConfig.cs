using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MyJobLeads.DomainModel.Entities.Metrics;

namespace MyJobLeads.DomainModel.Entities.Configuration
{
    public class MilestoneConfig
    {
        public MilestoneConfig()
        {
            JobSearchMetrics = new JobSearchMetrics();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public MilestoneConfig NextMilestone { get; set; }
        public bool IsStartingMilestone { get; set; }

        [StringLength(Int32.MaxValue)]
        public string Instructions { get; set; }

        [StringLength(Int32.MaxValue)]
        public string CompletionDisplay { get; set; }

        // Milestone Completion Values
        public JobSearchMetrics JobSearchMetrics { get; set; }
    }
}
