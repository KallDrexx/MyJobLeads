using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.FillPerfect
{
    public class FpOrgPilotUsedLicense
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime LicenseGrantDate { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
