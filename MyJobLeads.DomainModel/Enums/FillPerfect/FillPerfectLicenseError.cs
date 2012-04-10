using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Enums
{
    public enum FillPerfectLicenseError
    {
        None = 0,
        InvalidKey = 1,
        KeyNotActivated = 2,
        NoLicense = 3
    }
}
