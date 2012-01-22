using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.CompanySearching
{
    public class ExternalCompanyDetailsViewModel
    {
        public string CompanyId { get; set; }
        public string CompanName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set }
        public string Zip { get; set; }
        public string Overview { get; set; }
        public long Revenue { get; set; }
        public string Ownership { get; set; }
        public int EmployeeCount { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Industry1 { get; set; }
        public string SubIndustry1 { get; set; }
        public string Industry2 { get; set; }
        public string SubIndustry2 { get; set; }
        public string Industry3 { get; set; }
        public string SubIndustry3 { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
