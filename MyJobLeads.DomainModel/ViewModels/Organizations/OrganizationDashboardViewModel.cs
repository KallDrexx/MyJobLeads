using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.ViewModels.Organizations
{
    public class OrganizationDashboardViewModel
    {
        public Organization Organization { get; set; }
        public IList<OfficialDocument> NonMemberOfficialDocuments { get; set; }
    }
}
