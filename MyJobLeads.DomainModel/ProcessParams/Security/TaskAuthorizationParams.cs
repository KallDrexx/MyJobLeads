using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Security
{
    public struct TaskAuthorizationParams
    {
        public int RequestingUserId { get; set; }
        public int TaskId { get; set; }
    }
}
