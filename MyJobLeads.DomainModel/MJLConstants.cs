using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel
{
    public class MJLConstants
    {
        // History Constants
        public const string HistoryInsert = "ins";
        public const string HistoryUpdate = "upd";
        public const string HistoryDelete = "del";

        // Task Categories
        public const string ApplyToFirmTaskCategory = "Apply At Firm";
        public const string FollowUpTaskCategory = "Follow Up";
        public const string InPersonInterviewTaskCategory = "In-Person Interview";
        public const string InitialContactTaskCategory = "Initial Contact";
        public const string PhoneInterviewTaskCategory = "Phone Interview";
        public const string OtherTaskCategory = "Other";

        // Company Lead Statuses
        public const string ProspectiveEmployerCompanyStatus = "Prospective Employer";
        public const string DeadLeadCompanyStatus = "Dead Lead";
        public const string InterviewingCompanyStatus = "In Process (Interviewing)";
    }
}
