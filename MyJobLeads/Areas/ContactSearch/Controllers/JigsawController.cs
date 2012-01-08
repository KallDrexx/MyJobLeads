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

        public JigsawController(MyJobLeadsDbContext context,
                                IProcess<GetJigsawUserPointsParams, JigsawUserPointsViewModel> getUserPointsProc,
                                IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel> saveCredentialsProc,
                                IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel> searchContactsProc,
                                IProcess<AddJigsawContactToJobSearchParams, ExternalContactAddedResultViewModel> addContactProc)
        {
            _context = context;
            _getUserPointsProc = getUserPointsProc;
            _saveCredentialsProc = saveCredentialsProc;
            _searchContacsProc = searchContactsProc;
            _addContactProc = addContactProc;
        }

        public virtual ActionResult Index()
        {
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
            // Get all contacts for the user
            var contacts = _context.Contacts
                                   .Where(x => x.Company.JobSearch.User.Id == CurrentUserId)
                                   .Include(x => x.Company)
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
            model.SetExistingContactList(contacts);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult ImportContact(ImportContactViewModel model)
        {
            if (model.ExistingContactId <= 0 && !model.CreateNewContact)
                ModelState.AddModelError("", "An existing contact must be selected to merge, or you must mark this as a new contact");

            if (!ModelState.IsValid)
            {
                // Get all contacts for the user
                var contacts = _context.Contacts
                                       .Where(x => x.Company.JobSearch.User.Id == CurrentUserId)
                                       .Include(x => x.Company)
                                       .OrderBy(x => x.Name)
                                       .ToList();
                model.SetExistingContactList(contacts);

                return View(model);
            }

            if (model.CreateNewContact)
                return RedirectToAction(MVC.ContactSearch.Jigsaw.AddContact(model.JigsawContactId, model.JigsawCompanyId, model.CompanyName, model.ContactName, model.ContactTitle));

            // Otherwie merge
            return RedirectToAction(MVC.ContactSearch.Sync.Jigsaw(model.ExistingContactId, model.JigsawContactId, model.ContactName, model.ContactTitle, model.JigsawUpdatedDate));
        }

        public virtual ActionResult AddContact(int jsContactId, string jsCompanyId, string jsCompanyName, string name, string title)
        {
            var user = _context.Users
                               .Where(x => x.Id == CurrentUserId)
                               .Include(x => x.LastVisitedJobSearch.Companies)
                               .Single();

            var model = new AddJigsawContactViewModel
            {
                JigsawContactId = jsContactId,
                JigsawCompanyId = jsCompanyId,
                JigsawCompanyName = jsCompanyName,
                Name = name,
                Title = title
            };
            model.SetExistingCompanyList(user.LastVisitedJobSearch.Companies.ToList());

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult AddContact(AddJigsawContactViewModel model)
        {
            if (!model.CreateNewCompany && model.SelectedCompanyId == 0)
                ModelState.AddModelError("CompanyName", "No existing company selected");

            if (!ModelState.IsValid)
            {
                var user = _context.Users
                               .Where(x => x.Id == CurrentUserId)
                               .Include(x => x.LastVisitedJobSearch.Companies)
                               .Single();

                model.SetExistingCompanyList(user.LastVisitedJobSearch.Companies.ToList());
                return View(model);
            }

            var parameters = Mapper.Map<AddJigsawContactViewModel, AddJigsawContactToJobSearchParams>(model);
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
    }
}
