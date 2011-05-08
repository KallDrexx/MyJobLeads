using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.ViewModels
{
    public class PerformedSearchViewModel
    {
        public string SearchQuery { get; set; }
        public SearchResultEntities Results { get; set; }
    }
}