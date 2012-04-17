using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.Ordering;

namespace MyJobLeads.DomainModel.ViewModels.FillPerfect
{
    public class FpLicensesAvailableForOrderingViewModel
    {
        public IList<AvailableFpLicense> Licenses { get; set; }
        public bool OrganizationLicenseAvailable { get; set; }
        public string OrgName { get; set; }

        public class AvailableFpLicense
        {
            public int ProductId { get; set; }
            public decimal Price { get; set; }
            public string ProductType { get; set; }
            public int DurationInWeeks { get; set; }
            public bool PurchasedTooManyTimes { get; set; }
        }
    }
}
