using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;

namespace MyJobLeads.Areas.PositionSearch.Models
{
    public class PerformedSearchViewModel
    {
        public PositionSearchQueryViewModel SearchQuery { get; set; }
        public PositionSearchResultsViewModel Results { get; set; }
    }
}