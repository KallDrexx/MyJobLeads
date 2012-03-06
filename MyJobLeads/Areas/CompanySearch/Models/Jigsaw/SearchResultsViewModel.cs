using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ViewModels.CompanySearching;

namespace MyJobLeads.Areas.CompanySearch.Models.Jigsaw
{
    public class SearchResultsViewModel
    {
        public CompanySearchQueryViewModel Query { get; set; }
        public SearchResultsViewModel<ExternalCompanySearchResultViewModel> Results { get; set; }
    }
}