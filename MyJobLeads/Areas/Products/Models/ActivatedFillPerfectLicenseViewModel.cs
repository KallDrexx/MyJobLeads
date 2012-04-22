using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.Areas.Products.Models
{
    public class ActivatedFillPerfectLicenseViewModel
    {
        public Guid UserFpKey { get; set; }
        public DateTime LicenseEffectiveDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public string LicenseType { get; set; }
    }
}