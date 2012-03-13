using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.FillPerfect
{
    public class SendFpReplyParams
    {
        public int RequestingUserId { get; set; }
        public int ResponseId { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string EmailContent { get; set; }
    }
}
