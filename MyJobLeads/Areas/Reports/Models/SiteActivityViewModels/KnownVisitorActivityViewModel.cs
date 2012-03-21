using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.Reports.Models.SiteActivityViewModels
{
    public class KnownVisitorActivityViewModel
    {
        public string Name { get; set; }
        public List<VisitorActivityViewModel> Activity { get; set; }

        public class VisitorActivityViewModel
        {
            public string Url { get; set; }
            public DateTime VisitDate { get; set; }
        }
    }
}