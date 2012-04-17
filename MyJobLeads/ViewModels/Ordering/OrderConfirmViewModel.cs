using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.ViewModels.Ordering
{
    public class OrderConfirmViewModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int LicenseDurationInWeeks { get; set; }
        public string LicenseType { get; set; }
        public DateTime? LicenseEffectiveDate { get; set; }
        public bool AnotherLicenseAlreadyActive { get; set; }
        public bool MaxPurchaseExceeded { get; set; }
    }
}