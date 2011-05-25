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

namespace MyJobLeads.Controllers
{
    [MJLAuthorize]
    public partial class CompanyController : MyJobLeadsBaseController
    {
        protected ISearchProvider _searchProvider;

        public CompanyController(IUnitOfWork unitOfWork, ISearchProvider searchProvider)
        {
            _unitOfWork = unitOfWork;
            _searchProvider = searchProvider;
        }

        public virtual ActionResult Add(int jobSearchId)
        {
            ViewBag.JobSearchId = jobSearchId;
            return View(MVC.Company.Views.Edit, new Company());
        }

        public virtual ActionResult Edit(int id)
        {
            ViewBag.JobSearchId = 0;
            var company = new CompanyByIdQuery(_unitOfWork).WithCompanyId(id).Execute();
            return View(company);
        }

        [HttpPost]
        public virtual ActionResult Edit(Company company, int jobSearchId)
        {
            // Determine if this is a new company or not
            if (company.Id == 0)
            {
                company = new CreateCompanyCommand(_unitOfWork, _searchProvider).WithJobSearch(jobSearchId)
                                                                      .SetName(company.Name)
                                                                      .SetCity(company.City)
                                                                      .SetIndustry(company.Industry)
                                                                      .SetMetroArea(company.MetroArea)
                                                                      .SetNotes(company.Notes)
                                                                      .SetPhone(company.Phone)
                                                                      .SetState(company.State)
                                                                      .SetZip(company.Zip)
                                                                      .CalledByUserId(CurrentUserId)
                                                                      .Execute();
            }
            else
            {
                company = new EditCompanyCommand(_unitOfWork, _searchProvider).WithCompanyId(company.Id)
                                                                      .SetName(company.Name)
                                                                      .SetCity(company.City)
                                                                      .SetIndustry(company.Industry)
                                                                      .SetMetroArea(company.MetroArea)
                                                                      .SetNotes(company.Notes)
                                                                      .SetPhone(company.Phone)
                                                                      .SetState(company.State)
                                                                      .SetZip(company.Zip)
                                                                      .RequestedByUserId(CurrentUserId)
                                                                      .Execute();
            }

            return RedirectToAction(MVC.Company.Details(company.Id));
        }

        public virtual ActionResult Details(int id)
        {
            var company = new CompanyByIdQuery(_unitOfWork).WithCompanyId(id).Execute();
            return View(company);
        }
    }
}
