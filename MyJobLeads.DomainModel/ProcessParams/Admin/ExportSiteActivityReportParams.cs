using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Admin
{
    public class ExportSiteActivityReportParams
    {
        public ExportSiteActivityReportParams()
        {
            IgnoredUsers = new List<string>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> IgnoredUsers { get; set; }
        public bool SortDateDescending { get; set; }
    }
}
