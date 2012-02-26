using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class SiteReferral
    {
        public int Id { get; set; }
        public string ReferralCode { get; set; }
        public string Url { get; set; }
        public string IpAddress { get; set; }
        public DateTime Date { get; set; }
    }
}
