using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Security
{
    public class OrganizationAdminAuthorizationParams
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
    }
}
