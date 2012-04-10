using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.FillPerfect
{
    public class ActivateFillPerfectKeyParams
    {
        public Guid FillPerfectKey { get; set; }
        public string MachineId { get; set; }
    }
}
