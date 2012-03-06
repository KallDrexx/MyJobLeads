using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class SiteReferralCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string GivenTo { get; set; }
    }
}
