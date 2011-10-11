using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Organizations
{
    public struct OrgMemberDocVisibilityByOrgAdminParams
    {
        public int RequestingUserId { get; set; }
        public int OfficialDocumentId { get; set; }
        public bool ShowToMembers { get; set; }
    }
}
