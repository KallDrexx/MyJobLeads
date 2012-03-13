using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.FillPerfect.Models.ContactUsResponses
{
    public class ContactUsSendReplyViewModel
    {
        public int ResponseId { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string ToPassword { get; set; }
        public string ToOrgName { get; set; }
        public string FromAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public bool ShowEditView { get; set; }
    }
}