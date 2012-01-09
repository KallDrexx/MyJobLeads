using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class JigsawContactNotOwnedException : JigsawException
    {
        public JigsawContactNotOwnedException(string contactId, int userId)
            : base(string.Format("User {0}'s jigsaw account does not own the jigsaw contact {1}", userId, contactId))
        {
            ContactId = contactId;
            UserId = userId;
        }

        public string ContactId { get; set; }
        public int UserId { get; set; }
    }
}
