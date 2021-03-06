﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.Areas.ContactSearch.Models.Jigsaw;

namespace MyJobLeads.Areas.ContactSearch.Controllers
{
    [RequiresActiveJobSearch]
    public partial class JigsawController : MyJobLeadsBaseController
    {
        protected IProcess<GetJigsawUserPointsParams, JigsawUserPointsViewModel> _getUserPointsProc;
        protected IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel> _saveCredentialsProc;
        protected IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel> _searchContacsProc;
        protected IProcess<AddJigsawContactToJobSearchParams, ExternalContactAddedResultViewModel> _addContactProc;
        protected IProcess<GetJigsawContactDetailsParams, ExternalContactSearchResultsViewModel.ContactResultViewModel> _getContactProc;

        public JigsawController(MyJobLeadsDbContext context,
                                IProcess<GetJigsawUserPointsParams, JigsawUserPointsViewModel> getUserPointsProc,
                                IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel> saveCredentialsProc,
                                IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel> searchContactsProc,
                                IProcess<AddJigsawContactToJobSearchParams, ExternalContactAddedResultViewModel> addContactProc,
                                IProcess<GetJigsawContactDetailsParams, ExternalContactSearchResultsViewModel.ContactResultViewModel> getContactProc)
        {
            _context = context;
            _getUserPointsProc = getUserPointsProc;
            _saveCredentialsProc = saveCredentialsProc;
            _searchContacsProc = searchContactsProc;
            _addContactProc = addContactProc;
            _getContactProc = getContactProc;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Authenticate(string returnUrl = "")
        {
            return View(new JigsawAuthenticateViewModel { ReturnUrl = returnUrl });
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

            // Authentication was successful, go to the return url if one specified
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                return Redirect(model.ReturnUrl);
            
            return RedirectToAction(MVC.ContactSearch.Jigsaw.Index());
        }

        public virtual ActionResult PerformSearch(JigsawSearchParametersViewModel model)
        {
            ExternalContactSearchResultsViewModel results;
            var parameters = Mapper.Map<JigsawSearchParametersViewModel, JigsawContactSearchParams>(model);
            parameters.RequestingUserId = CurrentUserId;
            results = _searchContacsProc.Execute(parameters);

            var resultsModel = new JigsawSearchResultsViewModel
            {
                Query = model,
                Results = results
            };

            return View(resultsModel);
        }

        public virtual ActionResult ImportContact(int jsContactId, string jsCompanyId, string jsCompanyName, string name, string title, DateTime lastUpdated)
        {
            // Get all contacts and contacts for the user
            var contacts = _context.Contacts
                                   .Where(x => x.Company.JobSearch.User.Id == CurrentUserId)
                                   .Include(x => x.Company)
                                   .ToList();

            var companies = _context.Companies
                                    .Where(x => x.JobSearch.User.Id == CurrentUserId)
                                    .ToList();

            var model = new ImportContactViewModel
            {
                CompanyName = jsCompanyName,
                JigsawCompanyId = jsCompanyId,
                ContactName = name,
                ContactTitle = title,
                JigsawContactId = jsContactId,
                JigsawUpdatedDate = lastUpdated
            };

            model.SetDropdownListValues(contacts, companies);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult ImportContact(ImportContactViewModel model)
        {
            if (!model.CreateNewContact && model.SelectedContactId <= 0)
                ModelState.AddModelError("", "An existing contact must be selected to merge, or you must mark this as a new contact");

            else if (model.CreateNewContact && !model.CreateNewCompany && model.SelectedCompanyId <= 0)
                ModelState.AddModelError("", "An existing company must be selected to add the contact to an existing company");

            if (!ModelState.IsValid)
            {
                // Get all contacts for the user
                var contacts = _context.Contacts
                                       .Where(x => x.Company.JobSearch.User.Id == CurrentUserId)
                                       .Include(x => x.Company)
                                       .OrderBy(x => x.Name)
                                       .ToList();

                model.SetDropdownListValues(contacts, contacts.Select(x => x.Company).Distinct().ToList());

                return View(model);
            }

            // Pass to the merge screen if we are not creating a new contact
            if (!model.CreateNewContact)
                return RedirectToAction(MVC.ContactSearch.Sync.Jigsaw(model.SelectedContactId, model.JigsawContactId, model.ContactName, model.ContactTitle, model.JigsawUpdatedDate));

            // Otherwise create the new contact
            var parameters = Mapper.Map<ImportContactViewModel, AddJigsawContactToJobSearchParams>(model);
            parameters.RequestingUserId = CurrentUserId;
            var result = _addContactProc.Execute(parameters);

            return RedirectToAction(MVC.ContactSearch.Jigsaw.AddContactSuccessful(result.ContactId, result.ContactName, result.CompanyName, result.CompanyId));
        }

        public virtual ActionResult AddContactSuccessful(int contactId, string contactName, string companyName, int companyId)
        {
            var model = new ExternalContactAddedResultViewModel
            {
                ContactId = contactId,
                ContactName = contactName,
                CompanyId = companyId,
                CompanyName = companyName
            };

            return View(model);
        }

        public virtual ActionResult PurchaseContact(int jigsawId, string contactName, string prevUrl)
        {
            JigsawUserPointsViewModel points;

            try { points = _getUserPointsProc.Execute(new GetJigsawUserPointsParams { RequestingUserId = CurrentUserId }); }
            catch (JigsawException ex)
            {
                if (!(ex is JigsawCredentialsNotFoundException) && !(ex is InvalidJigsawCredentialsException))
                    throw;

                return RedirectToAction(MVC.ContactSearch.Jigsaw.Authenticate(Request.RawUrl));
            }

            if (points == null)
                throw new JigsawException("User Jigsaw Points structure was null");

            var model = new JigsawContactPurchaseViewModel
            {
                Points = points.Points,
                PurchasePointsUrl = points.PurchasePointsUrl,
                JigsawContactId = jigsawId,
                JigsawContactName = contactName,
                ReturnUrl = prevUrl
            };

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult PurchaseContact(JigsawContactPurchaseViewModel model)
        {
            if (!model.PurchaseContact)
                ModelState.AddModelError("", "You must confirm your purchase request");

            if (!ModelState.IsValid)
            {
                // Refresh points information
                var points = _getUserPointsProc.Execute(new GetJigsawUserPointsParams { RequestingUserId = CurrentUserId });
                model.Points = points.Points;
                model.PurchasePointsUrl = points.PurchasePointsUrl;
                return View(model);
            }

            try
            {
                _getContactProc.Execute(new GetJigsawContactDetailsParams
                {
                    RequestingUserId = CurrentUserId,
                    JigsawContactId = model.JigsawContactId,
                    PurchaseContact = true
                });
            }

            catch (InsufficientJigsawPointsException)
            {
                ModelState.AddModelError("", "Your Jigsaw account does not have enough points to make this purchase.");
                var points = _getUserPointsProc.Execute(new GetJigsawUserPointsParams { RequestingUserId = CurrentUserId });
                model.Points = points.Points;
                model.PurchasePointsUrl = points.PurchasePointsUrl;
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction(MVC.ContactSearch.Jigsaw.Index());
        }
    }
}
