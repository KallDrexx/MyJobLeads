using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.ContactSearch.Models.Jigsaw
{
    public class JigsawContactPurchaseViewModel
    {
        public int Points { get; set; }
        public string PurchasePointsUrl { get; set; }
        public string ReturnUrl { get; set; }
        public int JigsawContactId { get; set; }
        public string JigsawContactName { get; set; }
        public bool PurchaseContact { get; set; }
    }
}