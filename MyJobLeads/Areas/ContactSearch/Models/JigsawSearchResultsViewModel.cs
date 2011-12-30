using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;

namespace MyJobLeads.Areas.ContactSearch.Models
{
    public class JigsawSearchResultsViewModel
    {
        public JigsawSearchParametersViewModel Query { get; set; }
        public ExternalContactSearchResultsViewModel Results { get; set; }
    }
}