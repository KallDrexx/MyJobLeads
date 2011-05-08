using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Commands.Search;

namespace MyJobLeads.Controllers
{
    public partial class SearchController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;

        public SearchController(IUnitOfWork unitOfWork, ISearchProvider searchProvider)
        {
            _unitOfWork = unitOfWork;
            _searchProvider = searchProvider;
        }

        public virtual ActionResult RefreshSearchIndex()
        {
            // Perform the re-index command
            new RefreshSearchIndexCommand(_unitOfWork, _searchProvider).Execute();
            return View();
        }
    }
}
