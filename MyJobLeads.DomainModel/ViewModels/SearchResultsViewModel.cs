using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels
{
    public class SearchResultsViewModel<TResults> where TResults : class
    {
        public int DisplayedPageNumber { get; set; }
        public int TotalResultsCount { get; set; }
        public int PageSize { get; set; }
        public IList<TResults> Results { get; set; }
    }
}
