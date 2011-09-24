using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.ProcessParams.Users
{
    public struct EditUserDetailsParams
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}
