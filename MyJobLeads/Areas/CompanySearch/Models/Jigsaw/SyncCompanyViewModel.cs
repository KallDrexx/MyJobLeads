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

        public string JigsawName { get; set; }
        public string JigsawAddress { get; set; }
        public string JigsawIndustry { get; set; }
        public string JigsawPhone { get; set; }

        public bool ImportName { get; set; }
        public bool ImportAddress { get; set; }
        public bool ImportIndustry { get; set; }
        public bool ImportPhone { get; set; }
    }
}