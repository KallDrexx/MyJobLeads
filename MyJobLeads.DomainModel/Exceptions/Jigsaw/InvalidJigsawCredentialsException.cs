using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class InvalidJigsawCredentialsException : JigsawException
    {
        public InvalidJigsawCredentialsException(int userId)
            : base(string.Format("The jigsaw credentials for user {0} are invalid", userId))
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
