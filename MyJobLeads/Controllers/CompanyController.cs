using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Companies;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.ViewModels.Companies;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Commands.JobSearches;
using FluentValidation;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    [RequiresActiveJobSearch]
    public partial class CompanyController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;
        protected IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> _companyAuthProcess;
        protected UserByIdQuery _userByIdQuery;

        public CompanyController(IUnitOfWork unitOfWork, 
                                 ISearchProvider searchProvider, 
                                 IServiceFactory serviceFactory,
                                 UserByIdQuery userByIdQuery,
                                 IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> compAuthProcess)
        {
            _unitOfWork = unitOfWork;
            _searchProvider = searchProvider;
            _serviceFactory = serviceFactory;
            _companyAuthProcess = compAuthProcess;
            _userByIdQuery = userByIdQuery;
        }

        public virtual ActionResult List()
        {
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            var model = new JobSearchCompanyListViewModel(user.LastVisitedJobSearch);

            return View(model);
        }

        public virtual ActionResult Add()
        {
            var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();
            var statuses = _serviceFactory.GetService<LeadStatusesAvailableForCompaniesQuery>().Execute();
            var model = new EditCompanyViewModel
            {
                JobSearchId = Convert.ToInt32(user.LastVisitedJobSearchId),
                AvailableLeadStatuses = _serviceFactory.GetService<LeadStatusesAvailableForCompaniesQuery>().Execute()
            };

            return View(MVC.Company.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var company = new CompanyByIdQuery(_unitOfWork, _companyAuthProcess).WithCompanyId(id).RequestedByUserId(CurrentUserId).Execute();
            if (company == null)
            {
                ViewBag.EntityType = "Company";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var statuses = _serviceFactory.GetService<LeadStatusesAvailableForCompaniesQuery>().Execute();
            return View(new EditCompanyViewModel(company) { AvailableLeadStatuses = statuses });
        }

        [HttpPost]
        public virtual ActionResult Edit(EditCompanyViewModel model)
        {
            Company company;

            try
            {
                // Determine if this is a new company or not
                if (model.Id == 0)
                {
                    company = new CreateCompanyCommand(_serviceFactory).WithJobSearch(model.JobSearchId)
                                                                          .SetName(model.Name)
                                                                          .SetCity(model.City)
                                                                          .SetIndustry(model.Industry)
                                                                          .SetMetroArea(model.MetroArea)
                                                                          .SetNotes(model.Notes)
                                                                          .SetPhone(model.Phone)
                                                                          .SetState(model.State)
                                                                          .SetZip(model.Zip)
                                                                          .CalledByUserId(CurrentUserId)
                                                                          .Execute();
                }
                else
                {
                    company = new EditCompanyCommand(_serviceFactory).WithCompanyId(model.Id)
                                                                          .SetName(model.Name)
                                                                          .SetCity(model.City)
                                                                          .SetIndustry(model.Industry)
                                                                          .SetMetroArea(model.MetroArea)
                                                                          .SetNotes(model.Notes)
                                                                          .SetPhone(model.Phone)
                                                                          .SetState(model.State)
                                                                          .SetZip(model.Zip)
                                                                          .SetLeadStatus(model.LeadStatus)
                                                                          .RequestedByUserId(CurrentUserId)
                                                                          .Execute();
                }

                return RedirectToAction(MVC.Company.Details(company.Id));
            }

            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return View(model);
            }
        }

        public virtual ActionResult Details(int id, bool showPositions = false)
        {
            var company = new CompanyByIdQuery(_unitOfWork, _companyAuthProcess).WithCompanyId(id)
                                                                                .RequestedByUserId(CurrentUserId)
                                                                                .Execute();
            if (company == null)
            {
                ViewBag.EntityType = "Company";
                return View(MVC.Shared.Views.EntityNotFound);
            }

            var model = new CompanyDisplayViewModel(company) { showPositions = showPositions };
            return View(model);
        }

        public virtual ActionResult ChangeCompanyStatusFilters(int jobSearchId, string[] shownStatus)
        {
            // If no statuses were marked to be shown (thus shownStatus is null) make it an empty array
            if (shownStatus == null)
                shownStatus = new string[0];

            var companies = _serviceFactory.GetService<CompaniesByJobSearchIdQuery>()
                                           .WithJobSearchId(jobSearchId)
                                           .Execute();

            var usedStatuses = companies.Select(x => x.LeadStatus).Distinct();

            var cmd = _serviceFactory.GetService<EditJobSearchCommand>()
                                     .WithJobSearchId(jobSearchId)
                                     .CalledByUserId(CurrentUserId)
                                     .ResetHiddenCompanyStatusList();

            // For each status that isn't listed as a shown status, mark it as hidden in the job search preferences
            foreach (string status in usedStatuses)
                if (!shownStatus.Contains(status))
                    cmd.HideCompanyStatus(status);

            // Execute the command and go back to the company list
            cmd.Execute();

            return RedirectToAction(MVC.Company.List());
        }
    }
}
