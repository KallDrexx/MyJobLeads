using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.ContactSearch.Models
{
    public class JigsawAuthenticateViewModel
    {
        public bool LoginFailed { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}