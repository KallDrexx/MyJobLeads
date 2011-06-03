using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels
{
    public class JobSearchUserMetrics
    {
        public int NumTasks { get; set; }
        public int NumTasksCompleted { get; set; }
        public int NumCompanies { get; set; }
        public int NumContacts { get; set; }
    }
}
