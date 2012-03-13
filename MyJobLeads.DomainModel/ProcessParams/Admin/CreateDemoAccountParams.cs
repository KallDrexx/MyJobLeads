using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Admin
{
    public class CreateDemoAccountParams
    {
        public int RequestingUserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OrganizationName { get; set; }
        public string Password { get; set; }
    }
}
