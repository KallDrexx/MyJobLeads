using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.ViewModels.JobSearches
{
    public class JobSearchDetailsViewModel
    {
        public JobSearch JobSearch { get; set; }
        public IList<Task> OpenTasks { get; set; }
        public JobSearchUserMetrics Metrics { get; set; }
    }
}