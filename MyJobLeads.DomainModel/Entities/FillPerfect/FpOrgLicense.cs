using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.FillPerfect
{
    public class FpOrgLicense
    {
        public int Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int LicenseCount { get; set; }

        public virtual Organization Organization { get; set; }
        public int OrganizationId { get; set; }
    }
}
