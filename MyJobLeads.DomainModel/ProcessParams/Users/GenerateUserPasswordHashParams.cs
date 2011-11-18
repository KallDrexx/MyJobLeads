using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Users
{
    public class GenerateUserPasswordHashParams
    {
        public string Email { get; set; }
        public string PlainTextPassword { get; set; }
    }
}
