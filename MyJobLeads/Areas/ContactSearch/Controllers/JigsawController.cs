using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ExternalAuth;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuthorization.Jigsaw;
using MyJobLeads.Areas.ContactSearch.Models;
using MyJobLeads.DomainModel.ViewModels.JobSearches;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuth.Jigsaw;
using MyJobLeads.DomainModel.Exceptions.Jigsaw;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw;
using AutoMapper;

namespace MyJobLeads.Areas.ContactSearch.Controllers
{
    [RequiresActiveJobSearch]
    public partial class JigsawController : MyJobLeadsBaseController
    {
        protected IProcess<GetJigsawUserPointsParams, JigsawUserPointsViewModel> _getUserPointsProc;
        protected IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel> _saveCredentialsProc;
        protected IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel> _searchContacsProc;

        public JigsawController(MyJobLeadsDbContext context,
                                IProcess<GetJigsawUserPointsParams, JigsawUserPointsViewModel> getUserPointsProc,
                                IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel> saveCredentialsProc,
                                IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel> searchContactsProc)
        {
            _context = context;
            _getUserPointsProc = getUserPointsProc;
            _saveCredentialsProc = saveCredentialsProc;
            _searchContacsProc = searchContactsProc;
        }

        public virtual ActionResult Index()
        {
            JigsawUserPointsViewModel points;

            try { points = _getUserPointsProc.Execute(new GetJigsawUserPointsParams { RequestingUserId = CurrentUserId }); }
            catch (Exception ex)
            {
                if (ex is JigsawCredentialsNotFoundException || ex is InvalidJigsawCredentialsException)
                    return RedirectToAction(MVC.ContactSearch.Jigsaw.Authenticate(true));

                throw;
            }

            return View();
        }

        public virtual ActionResult Authenticate(bool loginFailed = false)
        {
            return View(new JigsawAuthenticateViewModel());
        }

        [HttpPost]
        public virtual ActionResult Authenticate(JigsawAuthenticateViewModel model)
        {
            try
            {
                _saveCredentialsProc.Execute(new SaveJigsawUserCredentialsParams
                {
                    RequestingUserId = CurrentUserId,
                    JigsawUsername = model.Username,
                    JigsawPassword = model.Password
                });
            }
            catch (InvalidJigsawCredentialsException)
            {
                ModelState.AddModelError("login", "The provided credentials could not be used to log into Jigsaw");
                return View(model);
            }

            // Authentication was successful
            return RedirectToAction(MVC.ContactSearch.Jigsaw.Index());
        }

        public virtual ActionResult PerformSearch(JigsawSearchParametersViewModel model)
        {
            ExternalContactSearchResultsViewModel results;
            var parameters = Mapper.Map<JigsawSearchParametersViewModel, JigsawContactSearchParams>(model);
            parameters.RequestingUserId = CurrentUserId;
            
            try { results = _searchContacsProc.Execute(parameters); }
            catch (Exception ex)
            {
                if (ex is JigsawCredentialsNotFoundException || ex is InvalidJigsawCredentialsException)
                    return RedirectToAction(MVC.ContactSearch.Jigsaw.Authenticate(true));

                throw;
            }

            var resultsModel = new JigsawSearchResultsViewModel
            {
                Query = model,
                Results = results
            };

            return View(resultsModel);
        }
    }
}
