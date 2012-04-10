using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.ViewModels.FillPerfect
{
    public class FillPerfectLicenseViewModel
    {
        public FillPerfectLicenseError Error { get; set; }
        public string LicenseXml { get; set; }
        public string KeyXml { get; set; }
    }
}
