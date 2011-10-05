using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Security
{
    public struct PositionAuthorizationParams
    {
        public int PositionId { get; set; }
        public int RequestingUserId { get; set; }
    }
}
