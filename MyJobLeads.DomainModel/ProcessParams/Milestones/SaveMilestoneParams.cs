using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.Metrics;

namespace MyJobLeads.DomainModel.ProcessParams.Milestones
{
    public class SaveMilestoneParams
    {
        public SaveMilestoneParams()
        {
            JobSearchMetrics = new JobSearchMetrics();
        }

        public int Id { get; set; }
        public int RequestingUserId { get; set; }
        public int OrganizationId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public string CompletionDisplay { get; set; }
        public int PreviousMilestoneId { get; set; }
        public JobSearchMetrics JobSearchMetrics { get; set; }
    }
}
