using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn
{
    public class LinkedInPositionSearchParams
    {
        public int RequestingUserId { get; set; }
        public int ResultsPageNum { get; set; }
        public string Keywords { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
    }
}
