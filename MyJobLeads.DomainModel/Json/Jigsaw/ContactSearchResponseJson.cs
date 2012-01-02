using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Json.Jigsaw
{
    public class ContactSearchResponseJson
    {
        public int TotalHits { get; set; }
        public List<ContactDetailsJson> Contacts { get; set; }
    }
}
