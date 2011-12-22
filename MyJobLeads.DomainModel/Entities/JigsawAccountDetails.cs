using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class JigsawAccountDetails
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        public virtual User AssociatedUser { get; set; }
        public int AssociatedUserId { get; set; }
    }
}
