using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw
{
    public class JigsawCompanySearchParams
    {
        public int RequestedPageNum { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string MetroArea { get; set; }
        public string Employees { get; set; }
        public string Revenue { get; set; }
        public string Ownership { get; set; }
    }
}
