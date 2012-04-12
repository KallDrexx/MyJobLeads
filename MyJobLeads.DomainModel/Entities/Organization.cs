using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Entities.FillPerfect;

namespace MyJobLeads.DomainModel.Entities
{
    public class Organization
    {
        public Organization()
        {
            Members = new List<User>();
            EmailDomains = new List<OrganizationEmailDomain>();
            MemberOfficialDocuments = new List<OfficialDocument>();
            MilestoneConfigurations = new List<MilestoneConfig>();
            FpUsedPilotLicenses = new List<FpOrgPilotUsedLicense>();
            FillPerfectLicenses = new List<FpOrgLicense>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid RegistrationToken { get; set; }
        public bool IsEmailDomainRestricted { get; set; }
        public string FillPerfectPilotKey { get; set; }
        public int FpPilotLicenseCount { get; set; }

        public virtual ICollection<User> Members { get; set; }
        public virtual ICollection<OrganizationEmailDomain> EmailDomains { get; set; }
        public virtual ICollection<OfficialDocument> MemberOfficialDocuments { get; set; }
        public virtual ICollection<MilestoneConfig> MilestoneConfigurations { get; set; }
        public virtual ICollection<FpOrgPilotUsedLicense> FpUsedPilotLicenses { get; set; }
        public virtual ICollection<FpOrgLicense> FillPerfectLicenses { get; set; }
    }
}
