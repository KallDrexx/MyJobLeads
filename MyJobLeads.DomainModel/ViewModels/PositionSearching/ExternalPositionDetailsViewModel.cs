using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.ViewModels.PositionSearching
{
    public class ExternalPositionDetailsViewModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public DateTime PostedDate { get; set; }
        public string ExperienceLevel { get; set; }
        public string JobType { get; set; }
        public string JobFunctions { get; set; }
        public string Industries { get; set; }
        public string Description { get; set; }

        public string JobPosterId { get; set; }
        public string JobPosterName { get; set; }
        public string JobPosterHeadline { get; set; }

        public ExternalDataSource DataSource { get; set; }
    }
}
