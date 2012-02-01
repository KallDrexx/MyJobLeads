using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.CompanySearching
{
    public class ExternalCompanyDetailsViewModel
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Overview { get; set; }
        public decimal Revenue { get; set; }
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
        public string SourceUrl { get; set; }

        public string SingleIndustry
        {
            get
            {
                string output = string.Empty;
                if (!string.IsNullOrWhiteSpace(Industry1))
                {
                    output += Industry1;
                    if (!string.IsNullOrWhiteSpace(SubIndustry1))
                        output += ": " + SubIndustry1;
                }

                if (!string.IsNullOrWhiteSpace(Industry2))
                {
                    if (!string.IsNullOrWhiteSpace(output))
                        output += ", ";

                    output += Industry2;
                    if (!string.IsNullOrWhiteSpace(SubIndustry2))
                        output += ": " + SubIndustry2;
                }

                if (!string.IsNullOrWhiteSpace(Industry3))
                {
                    if (!string.IsNullOrWhiteSpace(output))
                        output += ", ";

                    output += Industry3;
                    if (!string.IsNullOrWhiteSpace(SubIndustry3))
                        output += ": " + SubIndustry3;
                }

                return output;
            }
        }
    }
}
