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

        public class CompanyDetailsJson
        {
            public string stockSymbol { get; set; }
            public string phone { get; set; }
            public string state { get; set; }
            public string revenueRange { get; set; }
            public string industry3 { get; set; }
            public string industry2 { get; set; }
            public string industry1 { get; set; }
            public string graveyarded { get; set; }
            public string city { get; set; }
            public string subIndustry1 { get; set; }
            public string subIndustry2 { get; set; }
            public string subIndustry3 { get; set; }
            public string currentLogo { get; set; }
            public string employeeRange { get; set; }
            public string name { get; set; }
            public string linkInJigsaw { get; set; }
            public string fortuneRank { get; set; }
            public string revenue { get; set; }
            public string zip { get; set; }
            public string error { get; set; }
            public string website { get; set; }
            public string updatedDate { get; set; }
            public string employeeCount { get; set; }
            public string sicCode { get; set; }
            public string country { get; set; }
            public string createdOn { get; set; }
            public string ownership { get; set; }
            public string address { get; set; }
            public string activeContacts { get; set; }
            public string companyId { get; set; }
            public string stockExchange { get; set; }
            public string currentWiki { get; set; }
        }
    }
}