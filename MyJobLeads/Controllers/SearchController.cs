using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Commands.Search;
using MyJobLeads.DomainModel.Queries.Search;
using MyJobLeads.ViewModels;

namespace MyJobLeads.Controllers
{
    public partial class SearchController : MyJobLeadsBaseController
    {
        protected RefreshSearchIndexCommand _refreshSearchIndexCommand;
        protected EntitySearchQuery _entitySearchQuery;

        public SearchController(RefreshSearchIndexCommand refreshSearchIndexCommand, EntitySearchQuery entitySearchQuery)
        {
            _refreshSearchIndexCommand = refreshSearchIndexCommand;
            _entitySearchQuery = entitySearchQuery;
        }

        public virtual ActionResult RefreshSearchIndex()
        {
            // Perform the re-index command
            _refreshSearchIndexCommand.Execute();
            return View();
        }

        public virtual ActionResult Search(string query)
        {
            // Perform the search
            var result = _entitySearchQuery.WithSearchQuery(query).Execute();
            return View(new PerformedSearchViewModel { SearchQuery = query, Results = result });
        }
    }
}
