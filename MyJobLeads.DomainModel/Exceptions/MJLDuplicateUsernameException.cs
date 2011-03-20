using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLDuplicateUsernameException : Exception
    {
        public MJLDuplicateUsernameException(string username)
            : base(string.Concat("A user already exists with the username of '", username, "'"))
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
