using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.Ordering;

namespace MyJobLeads.DomainModel.Entities.FillPerfect
{
    public class FpUserLicense
    {
        public int Id { get; set; }
        public FillPerfectLicenseType LicenseType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual Order Order { get; set; }
        public int Orderid { get; set; }
    }
}
