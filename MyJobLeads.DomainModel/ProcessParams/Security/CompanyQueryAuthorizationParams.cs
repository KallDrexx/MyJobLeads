using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Security
{
    public struct CompanyQueryAuthorizationParams
    {
        public int RequestingUserId { get; set; }
        public int CompanyId { get; set; }
    }
}
