using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.Areas.PositionSearch.Models;
using MyJobLeads.DomainModel.Queries.Companies;

namespace MyJobLeads.Areas.PositionSearch.Controllers
{
    [RequiresActiveJobSearch]
    public partial class LinkedInController : MyJobLeadsBaseController
    {
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _verifyTokenProcess;
        protected IProcess<StartLinkedInUserAuthParams, GeneralSuccessResultViewModel> _startLiAuthProcess;
        protected IProcess<ProcessLinkedInAuthProcessParams, GeneralSuccessResultViewModel> _finishLiAuthProcess;
        protected IProcess<LinkedInPositionSearchParams, PositionSearchResultsViewModel> _searchProcess;
        protected IProcess<LinkedInPositionDetailsParams, ExternalPositionDetailsViewModel> _positionDetailsProcess;
        protected IProcess<AddLinkedInPositionParams, ExternalPositionAddedResultViewModel> _addPositionProcess;
        protected CompaniesByJobSearchIdQuery _companyByJobSearchQuery;

        public LinkedInController(MyJobLeadsDbContext context,
                                    IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> verifyTokenProcess,
                                    IProcess<StartLinkedInUserAuthParams, GeneralSuccessResultViewModel> startLiAuthProcess,
                                    IProcess<ProcessLinkedInAuthProcessParams, GeneralSuccessResultViewModel> finishLiAuthProcess,
                                    IProcess<LinkedInPositionSearchParams, PositionSearchResultsViewModel> searchProcess,
                                    IProcess<LinkedInPositionDetailsParams, ExternalPositionDetailsViewModel> positionDetailsProcess,
                                    IProcess<AddLinkedInPositionParams, ExternalPositionAddedResultViewModel> addPositionProcess,
                                    CompaniesByJobSearchIdQuery companyByJSQuery)
        {
            _context = context;
            _verifyTokenProcess = verifyTokenProcess;
            _startLiAuthProcess = startLiAuthProcess;
            _finishLiAuthProcess = finishLiAuthProcess;
            _searchProcess = searchProcess;
            _positionDetailsProcess = positionDetailsProcess;
            _companyByJobSearchQuery = companyByJSQuery;
            _addPositionProcess = addPositionProcess;
        }

        public virtual ActionResult Index()
        {
            // If the user doesn't have a valid LinkedIn OAuth token, redirect them to get it
            if (!_verifyTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = CurrentUserId }).AccessTokenValid)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.AuthorizationAlert());

            return View();
        }

        public virtual ActionResult PerformSearch(PositionSearchQueryViewModel searchParams, int pageNum = 0)
        {
            if (!_verifyTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = CurrentUserId }).AccessTokenValid)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.AuthorizationAlert());

            if (!ModelState.IsValid)
                return View(new PerformedSearchViewModel { SearchQuery = searchParams });

            var results = _searchProcess.Execute(new LinkedInPositionSearchParams
            {
                RequestingUserId = CurrentUserId,
                Keywords = searchParams.Keywords,
                CountryCode = searchParams.SelectedCountryCode,
                ZipCode = searchParams.PostalCode,
                ResultsPageNum = pageNum
            });

            var model = new PerformedSearchViewModel
            {
                SearchQuery = searchParams,
                Results = results
            };

            return View(model);
        }

        public virtual ActionResult PositionDetails(string id)
        {
            if (!_verifyTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = CurrentUserId }).AccessTokenValid)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.AuthorizationAlert());

            var position = _positionDetailsProcess.Execute(new LinkedInPositionDetailsParams
            {
                RequestingUserId = CurrentUserId,
                PositionId = id
            });

            return View(position);
        }

        public virtual ActionResult AddPosition(string positionId, string positionTitle, string companyName)
        {
            var user = _context.Users.Where(x => x.Id == CurrentUserId).Single();
            var companies = _companyByJobSearchQuery.WithJobSearchId((int)user.LastVisitedJobSearchId).Execute();
            var model = new AddPositionViewModel
            {
                CompanyName = companyName,
                PositionId = positionId,
                PositionTitle = positionTitle,
                DataSource = DomainModel.Enums.ExternalDataSource.LinkedIn
            };
            model.SetCompanyList(companies);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult AddPosition(AddPositionViewModel model)
        {
            if (!_verifyTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = CurrentUserId }).AccessTokenValid)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.AuthorizationAlert());

            if (!model.CreateNewCompany && model.SelectedCompany <= 0)
                ModelState.AddModelError("company", "You must either select to add a new company entry, or select an existing company to add the position to");

            if (!ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.Id == CurrentUserId).Single();
                var companies = _companyByJobSearchQuery.WithJobSearchId((int)user.LastVisitedJobSearchId).Execute();
                model.SetCompanyList(companies);
                return View(model);
            }

            var result = _addPositionProcess.Execute(new AddLinkedInPositionParams
            {
                RequestingUserId = CurrentUserId,
                CreateNewCompany = model.CreateNewCompany,
                PositionId = model.PositionId,
                ExistingCompanyId = model.SelectedCompany
            });

            return RedirectToAction(MVC.PositionSearch.LinkedIn.AddPositionSuccessful(result.CompanyId, result.CompanyName, result.PositionId, result.PositionTitle));
        }

        public virtual ActionResult AddPositionSuccessful(int companyId, string companyName, int positionId, string positionTitle)
        {
            var model = new ExternalPositionAddedResultViewModel
            {
                CompanyId = companyId,
                CompanyName = companyName,
                PositionId = positionId,
                PositionTitle = positionTitle
            };

            return View(model);
        }

        public virtual ActionResult AuthorizationAlert()
        {
            return View();
        }

        public virtual ActionResult BeginAuthorization()
        {
            _startLiAuthProcess.Execute(new StartLinkedInUserAuthParams
            {
                RequestingUserId = CurrentUserId,
                ReturnUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action(MVC.PositionSearch.LinkedIn.ProcessAuthorization()))
            });

            // Shouldn't be hit
            return null;
        }

        public virtual ActionResult ProcessAuthorization()
        {
            var result = _finishLiAuthProcess.Execute(new ProcessLinkedInAuthProcessParams
            {
                UserId = CurrentUserId
            });

            if (result.WasSuccessful)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.Index());

            return View();
        }

    }
}