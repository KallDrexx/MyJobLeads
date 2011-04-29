using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.ViewModels.JobSearches
{
    public class JobSearchDetailsViewModel
    {
        public JobSearch JobSearch { get; set; }
        public IList<Task> OpenTasks { get; set; }
    }
}