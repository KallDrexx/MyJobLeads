using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.ContactSearching
{
    public class LinkedInContactDetailsParams
    {
        public int RequestingUserId { get; set; }
        public string ContactId { get; set; }
        public string PublicUrl { get; set; }
    }
}
