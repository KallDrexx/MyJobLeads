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
            if (model == null)
                throw new ArgumentNullException("model");

            JobSearchId = model.JobSearch.Id;
            SetHiddenStatusList(model.JobSearch.HiddenCompanyStatuses);
            UsedStatuses = model.JobSearch.Companies.Select(x => x.LeadStatus).Distinct().ToList();
            TotalCompanyCount = model.JobSearch.Companies.Count();
            Companies = model.JobSearch.Companies.Where(x => !HiddenStatuses.Contains(x.LeadStatus)).ToList();
        }

        public JobSearchCompanyListViewModel(JobSearch jobSearch)
        {
            if (jobSearch == null)
                throw new ArgumentNullException("jobSearch");

            JobSearchId = jobSearch.Id;
            SetHiddenStatusList(jobSearch.HiddenCompanyStatuses);
            UsedStatuses = jobSearch.Companies.Select(x => x.LeadStatus).Distinct().ToList();
            TotalCompanyCount = jobSearch.Companies.Count();

            // Only show companies with statuses not on the hidden status list
            Companies = jobSearch.Companies.Where(x => !HiddenStatuses.Contains(x.LeadStatus)).ToList();
        }

        public void SetHiddenStatusList(string semicolonDelemitedList)
        {
           if (string.IsNullOrWhiteSpace(semicolonDelemitedList))
               HiddenStatuses = new List<string>();
           else
                HiddenStatuses = new List<string>(semicolonDelemitedList.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries));
        }

        public IList<Company> Companies { get; set; }
        public int JobSearchId { get; set; }
        public IList<string> HiddenStatuses { get; set; }
        public IList<string> UsedStatuses { get; set; }
        public int TotalCompanyCount { get; set; }
    }
}