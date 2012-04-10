using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Enums.FillPerfect
{
    public enum FillPerfectActivationResult
    {
        InvalidKey = 0,
        NoLicense = 1,
        KeyAlreadyActivated = 2,
        ActivationSuccessful = 3
    }
}
