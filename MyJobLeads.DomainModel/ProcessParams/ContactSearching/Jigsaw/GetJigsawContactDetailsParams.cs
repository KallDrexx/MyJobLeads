using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw
{
    public class GetJigsawContactDetailsParams
    {
        public int RequestingUserId { get; set; }
        public int JigsawContactId { get; set; }
        public bool PurchaseContact { get; set; }
    }
}
