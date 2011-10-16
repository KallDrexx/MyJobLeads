using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Organizations
{
    public class ByOrganizationIdParams
    {
        public int RequestingUserId { get; set; }
        public int OrganizationId { get; set; }
    }
}
