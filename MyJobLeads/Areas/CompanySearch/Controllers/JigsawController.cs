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
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.CompanySearch.Controllers
{   
    [RequiresActiveJobSearch]
    public partial class JigsawController : MyJobLeadsBaseController
    {
        protected IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>> _searchProcess;
        protected IProcess<JigsawCompanyDetailsParams, ExternalCompanyDetailsViewModel> _companyDetailsProcess;
        protected IProcess<AddJigsawCompanyParams, CompanyAddedViewResult> _addCompanyProcess;
        protected IProcess<MergeJigsawCompanyParams, GeneralSuccessResultViewModel> _mergeCompanyProcess;
        protected CompanyByIdQuery _companyByIdQuery;

        public JigsawController(MyJobLeadsDbContext context, 
                                IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>> searchProcess,
                                IProcess<JigsawCompanyDetailsParams, ExternalCompanyDetailsViewModel> companyDetailsProc,
                                IProcess<AddJigsawCompanyParams, CompanyAddedViewResult> addCompProc,
                                IProcess<MergeJigsawCompanyParams, GeneralSuccessResultViewModel> mergeCompProc,
                                CompanyByIdQuery compByIdQuery)
        {
            _context = context;
            _searchProcess = searchProcess;
            _companyDetailsProcess = companyDetailsProc;
            _addCompanyProcess = addCompProc;
            _companyByIdQuery = compByIdQuery;
            _mergeCompanyProcess = mergeCompProc;
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

        public virtual ActionResult Details(int jigsawId, int? mergeCompanyId = null)
        {
            var model = _companyDetailsProcess.Execute(new JigsawCompanyDetailsParams { RequestingUserId = CurrentUserId, JigsawId = jigsawId });
            ViewBag.MergeCompanyId = mergeCompanyId;
            return View(model);
        }

        public virtual ActionResult AddCompany(int jigsawId, string companyName)
        {
            var jobSearch = _context.Users.Where(x => x.Id == CurrentUserId).Select(x => x.LastVisitedJobSearch).Single();
            var model = new AddCompanyViewModel { JigsawId = jigsawId, JigsawCompanyName = companyName };
            model.CreateCompanyList(jobSearch);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult AddCompany(AddCompanyViewModel model)
        {
            if (model.ExistingCompanyId == 0 && !model.CreateNewCompany)
                ModelState.AddModelError("", "You must either mark to create a new company, or select a company to sync with");

            if (!ModelState.IsValid)
            {
                var jobSearch = _context.Users.Where(x => x.Id == CurrentUserId).Select(x => x.LastVisitedJobSearch).Single();
                model.CreateCompanyList(jobSearch);
                return View(model);
            }

            if (model.CreateNewCompany)
            {
                var result = _addCompanyProcess.Execute(new AddJigsawCompanyParams { JigsawId = model.JigsawId, RequestingUserId = CurrentUserId });
                return RedirectToAction(MVC.CompanySearch.Jigsaw.AddCompanySuccess(result.CompanyId, result.CompanyName));
            }

            // else
            return RedirectToAction(MVC.CompanySearch.Jigsaw.Sync(model.ExistingCompanyId, model.JigsawId));
        }

        public virtual ActionResult AddCompanySuccess(int companyId, string companyName)
        {
            var model = new AddCompanySuccessViewModel { CompanyId = companyId, CompanyName = companyName };
            return View(model);
        }

        public virtual ActionResult Sync(int companyId, int jigsawId)
        {
            var jigsawData = _companyDetailsProcess.Execute(new JigsawCompanyDetailsParams { RequestingUserId = CurrentUserId, JigsawId = jigsawId });
            var company = _companyByIdQuery.RequestedByUserId(CurrentUserId).WithCompanyId(companyId).Execute();

            if (company == null)
                throw new InvalidOperationException("Cannot sync jigsaw data with a null company");

            var model = Mapper.Map<Company, SyncCompanyViewModel>(company);
            model = Mapper.Map<ExternalCompanyDetailsViewModel, SyncCompanyViewModel>(jigsawData, model);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Sync(SyncCompanyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var mergeParams = Mapper.Map<SyncCompanyViewModel, MergeJigsawCompanyParams>(model);
            mergeParams.RequestingUserId = CurrentUserId;
            var result = _mergeCompanyProcess.Execute(mergeParams);

            return RedirectToAction(MVC.Company.Details(model.CompanyId));
        }
    }
}