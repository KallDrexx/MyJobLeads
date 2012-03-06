using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.FillPerfect.Models.Pilot
{
    public class OrgDashboardViewModel
    {
        public OrgDashboardViewModel()
        {
            LicensedUsers = new List<PilotLicenseUser>();
        }

        public int AlottedLicenseCount { get; set; }
        public int RemainingLicenseCount { get; set; }
        public IList<PilotLicenseUser> LicensedUsers { get; set; }

        public class PilotLicenseUser
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime GrantedDate { get; set; }
        }
    }
}