using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLIncorrectPasswordException : MJLException
    {
        public MJLIncorrectPasswordException(int userId)
            : base("An incorrect password was specified for user id " + userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
