using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn
{
    public class StartLinkedInUserAuthParams
    {
        public int RequestingUserId { get; set; }
        public Uri ReturnUrl { get; set; }
    }
}
