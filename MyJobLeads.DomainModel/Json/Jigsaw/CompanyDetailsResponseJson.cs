using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Json.Jigsaw
{
    public class CompanyDetailsResponseJson
    {
        public int totalHits { get; set; }
        public List<CompanyDetailsJson> companies { get; set; }
    }
}