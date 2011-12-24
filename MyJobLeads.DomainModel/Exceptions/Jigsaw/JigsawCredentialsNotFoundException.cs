using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class JigsawCredentialsNotFoundException : MJLException
    {
        public JigsawCredentialsNotFoundException(int userId)
            : base(string.Format("No JigSaw credentials were found for user {0}", userId))
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
