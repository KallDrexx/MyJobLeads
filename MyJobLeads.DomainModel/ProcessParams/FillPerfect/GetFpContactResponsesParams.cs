using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.FillPerfect
{
    public class GetFpContactResponsesParams
    {
        public int RequestingUserId { get; set; }
        public bool GetNewResponses { get; set; }
    }
}
