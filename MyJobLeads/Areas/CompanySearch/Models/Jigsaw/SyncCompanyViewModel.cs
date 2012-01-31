using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.CompanySearch.Models.Jigsaw
{
    public class SyncCompanyViewModel
    {
        public int JigsawId { get; set; }
        public int CompanyId { get; set; }
        public DateTime LastUpdatedOnJigaw { get; set; }

        public string InternalName { get; set; }
        public string InternalAddress { get; set; }
        public string InternalIndustry { get; set; }
        public string InternalPhone { get; set; }
        public string InternalWebsite { get; set; }

        public string JigsawName { get; set; }
        public string JigsawAddress { get; set; }
        public string JigsawIndustry { get; set; }
        public string JigsawPhone { get; set; }
        public string JigsawOwnership { get; set; }
        public string JigsawEmployeeCount { get; set; }
        public string JigsawRevenue { get; set; }
        public string JigsawWebsite { get; set; }

        public bool ImportName { get; set; }
        public bool ImportAddress { get; set; }
        public bool ImportIndustry { get; set; }
        public bool ImportPhone { get; set; }
        public bool ImportOwnership { get; set; }
        public bool ImportEmployeeCount { get; set; }
        public bool ImportRevenue { get; set; }
        public bool ImportWebsite { get; set; }
    }
}