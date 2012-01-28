using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw
{
    public class MergeJigsawCompanyParams
    {
        public int RequestingUserId { get; set; }
        public int JigsawCompanyId { get; set; }
        public int MyLeadsCompayId { get; set; }
        public bool MergeName { get; set; }
        public bool MergeAddress { get; set; }
        public bool MergeIndustry { get; set; }
        public bool MergePhone { get; set; }
        public bool MergeEmployeeCount { get; set; }
        public bool MergeRevenue { get; set; }
        public bool MergeWebsite { get; set; }
        public bool MergeOwnership { get; set; }
    }
}
