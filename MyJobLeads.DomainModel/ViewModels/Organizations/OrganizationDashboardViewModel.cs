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
        public IList<OfficialDocument> HiddenMemberDocuments { get; set; }

        public int NumMembers { get; set; }
        public int NumCompanies { get; set; }
        public int NumContacts { get; set; }
        public int NumOpenPhoneTasks { get; set; }
        public int NumClosedPhoneTasks { get; set; }
        public int NumOpenInterviewTasks { get; set; }
        public int NumClosedInterviewTasks { get; set; }
        public int NumNotAppliedPositions { get; set; }
        public int NumAppliedPositions { get; set; }
    }
}
