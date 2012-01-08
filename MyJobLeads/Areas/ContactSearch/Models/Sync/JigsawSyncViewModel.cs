using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.ContactSearch.Models.Sync
{
    public class JigsawSyncViewModel
    {
        public int ContactId { get; set; }
        public int JigsawId { get; set; }
        public DateTime LastUpdatedOnJigsaw { get; set; }

        public string InternalName { get; set; }
        public string InternalTitle { get; set; }
        public string InternalEmail { get; set; }
        public string InternalPhone { get; set; }

        public string JigsawName { get; set; }
        public string JigsawTitle { get; set; }
        public string JigsawEmail { get; set; }
        public string JigsawPhone { get; set; }

        public bool ImportName { get; set; }
        public bool ImportTitle { get; set; }
        public bool ImportEmail { get; set; }
        public bool ImportPhone { get; set; }
    }
}