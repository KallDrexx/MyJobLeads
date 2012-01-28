using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Areas.CompanySearch.Models.Jigsaw;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.CompanySearching;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw;
using AutoMapper;

namespace MyJobLeads.Areas.CompanySearch.Controllers
{
    public partial class JigsawController : MyJobLeadsBaseController
    {
        protected IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>> _searchProcess;

        public JigsawController(MyJobLeadsDbContext context, 
                                IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>> searchProcess)
        {
            _context = context;
            _searchProcess = searchProcess;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult PerformSearch(CompanySearchQueryViewModel query)
        {
            var parameters = Mapper.Map<CompanySearchQueryViewModel, JigsawCompanySearchParams>(query);
            parameters.RequestingUserId = CurrentUserId;

            var result = _searchProcess.Execute(parameters);
            var model = new SearchResultsViewModel { Query = query, Results = result };
            return View(model);
        }
    }
}