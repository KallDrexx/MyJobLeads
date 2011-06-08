using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Metrics
{
    public class JobSearchMetrics
    {
        public int NumCompaniesCreated { get; set; }
        public int NumContactsCreated { get; set; }
        public int NumApplyTasksCreated { get; set; }
        public int NumApplyTasksCompleted { get; set; }
        public int NumPhoneInterviewTasksCreated { get; set; }
        public int NumInPersonInterviewTasksCreated { get; set; }
    }
}
