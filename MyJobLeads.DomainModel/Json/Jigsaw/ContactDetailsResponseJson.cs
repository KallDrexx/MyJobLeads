using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Json.Jigsaw
{
    public class ContactDetailsResponseJson
    {
        public UnrecognizedContact Unrecognized { get; set; }
        public List<ContactDetailsJson> Contacts { get; set; }

        public int PointsUsed { get; set; }
        public int TotalHits { get; set; }
        public int NumberOfContactsPurchased { get; set; }
        public int PointBalance { get; set; }

        public class UnrecognizedContact
        {
            public IList<string> ContactId { get; set; }
        }
    }
}
