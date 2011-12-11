using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;

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
        public int PageSize { get; set; }
        public int PageNum { get; set; }

        public ExternalDataSource DataSource { get; set; }

        public class PositionSearchResultViewModel
        {
            public string JobId { get; set; }
            public string Title { get; set; }
            public string Company { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
        }
    }
}
