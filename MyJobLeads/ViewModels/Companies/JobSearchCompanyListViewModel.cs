using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.ViewModels.JobSearches;

namespace MyJobLeads.ViewModels.Companies
{
    public class JobSearchCompanyListViewModel
    {
        public JobSearchCompanyListViewModel() { }

        public JobSearchCompanyListViewModel(JobSearchDetailsViewModel model)
        {
            Companies = model.JobSearch.Companies.ToList();
            JobSearchId = model.JobSearch.Id;
        }

        public IList<Company> Companies { get; set; }
        public int JobSearchId { get; set; }
    }
}