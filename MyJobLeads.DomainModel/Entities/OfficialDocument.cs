using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class OfficialDocument
    {
        public OfficialDocument()
        {
            Organizations = new List<Organization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool MeantForMembers { get; set; }
        public string DownloadUrl { get; set; }

        public ICollection<Organization> Organizations { get; set; }
    }
}
