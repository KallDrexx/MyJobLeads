using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.PositionSearching
{
    public class PositionSearchResultsViewModel
    {
        public PositionSearchResultsViewModel()
        {
            Results = new List<PositionSearchResultViewModel>();
        }

        public IList<PositionSearchResultViewModel> Results { get; set; }

        public int TotalCount { get; set; }
        public int ResultsPageNum { get; set; }

        public class PositionSearchResultViewModel
        {
            public int JobId { get; set; }
            public string Headline { get; set; }
            public string Company { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
        }
    }
}
