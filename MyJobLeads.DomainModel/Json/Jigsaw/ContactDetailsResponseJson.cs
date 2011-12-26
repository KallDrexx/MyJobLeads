using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Json.Jigsaw
{
    public class ContactDetailsResponseJson
    {
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string AreaCode { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string SeoContactUrl { get; set; }
        public string State { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string ContactUrl { get; set; }
        public string Country { get; set; }
        public bool Owned { get; set; }
        public string City { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyId { get; set; }
        public string ContactId { get; set; }
    }
}
