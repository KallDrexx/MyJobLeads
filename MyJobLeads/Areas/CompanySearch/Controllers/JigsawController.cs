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
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.CompanySearch.Controllers
{   
    [RequiresActiveJobSearch]
    public partial class JigsawController : MyJobLeadsBaseController
    {
        protected IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>> _searchProcess;
        protected IProcess<JigsawCompanyDetailsParams, ExternalCompanyDetailsViewModel> _companyDetailsProcess;

        public JigsawController(MyJobLeadsDbContext context, 
                                IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>> searchProcess,
                                IProcess<JigsawCompanyDetailsParams, ExternalCompanyDetailsViewModel> companyDetailsProc)
        {
            _context = context;
            _searchProcess = searchProcess;
            _companyDetailsProcess = companyDetailsProc;
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

        public virtual ActionResult Details(int jigsawId)
        {
            var model = _companyDetailsProcess.Execute(new JigsawCompanyDetailsParams { RequestingUserId = CurrentUserId, JigsawId = jigsawId });
            return View(model);
        }
    }
}