using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLDuplicateEmailException : MJLException
    {
        public MJLDuplicateEmailException(string email)
            : base(string.Concat("A user already exists with the email of '", email, "'"))
        {
            Email = email;
        }

        public string Email { get; private set; }
    }
}
