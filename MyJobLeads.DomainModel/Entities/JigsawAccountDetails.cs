using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class JigsawAccountDetails
    {
        public int Id { get; set; } // This will equal the id value of the user it's associated with
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        public virtual User AssociatedUser { get; set; }
    }
}
