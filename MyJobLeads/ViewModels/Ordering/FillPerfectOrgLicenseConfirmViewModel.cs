using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.ViewModels.Ordering
{
    public class FillPerfectOrgLicenseConfirmViewModel
    {
        public string OrganizationName { get; set; }
        public DateTime EffectiveLicenseDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ActivatedLicenseExpiratioDate { get; set; }

        public bool OrderConfirmed { get; set; }
    }
}