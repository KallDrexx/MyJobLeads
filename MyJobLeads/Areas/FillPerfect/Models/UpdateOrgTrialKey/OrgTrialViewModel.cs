using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.FillPerfect.Models.UpdateOrgTrialKey
{
    public class OrgTrialViewModel
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string FillPerfectKey { get; set; }
        public int TotaltrialCount { get; set; }
    }
}