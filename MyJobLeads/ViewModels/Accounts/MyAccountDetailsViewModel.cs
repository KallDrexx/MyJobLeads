using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.ViewModels.Accounts
{
    public class MyAccountDetailsViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public Guid? FillPerfectKey { get; set; }
        public bool FillPerfectActivated { get; set; }
        public string OrganizationName { get; set; }
    }
}