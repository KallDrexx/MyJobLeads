using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Admin
{
    public class CreateOrgWithAdminUserParams
    {
        public string OrganizationName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminName { get; set; }
        public string AdminPlainTextPassword { get; set; }
        public int RequestingUserId { get; set; }
    }
}
