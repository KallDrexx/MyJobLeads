using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Companies;

namespace MyJobLeads.Controllers
{
    public partial class CompanyController : MyJobLeadsBaseController
    {
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var newCompany = new CreateCompanyCommand(_unitOfWork).WithJobSearch(jobSearchId)
                                                                      .SetName(company.Name)
                                                                      .SetCity(company.City)
                                                                      .SetIndustry(company.Industry)
                                                                      .SetMetroArea(company.MetroArea)
                                                                      .SetNotes(company.Notes)
                                                                      .SetPhone(company.Phone)
                                                                      .SetState(company.State)
                                                                      .SetZip(company.Zip)
                                                                      .Execute();
            }
            else
            {
                var editedCompany = new EditCompanyCommand(_unitOfWork).WithCompanyId(company.Id)
                                                                      .SetName(company.Name)
                                                                      .SetCity(company.City)
                                                                      .SetIndustry(company.Industry)
                                                                      .SetMetroArea(company.MetroArea)
                                                                      .SetNotes(company.Notes)
                                                                      .SetPhone(company.Phone)
                                                                      .SetState(company.State)
                                                                      .SetZip(company.Zip)
                                                                      .Execute();
            }

            return RedirectToAction(MVC.JobSearch.View(jobSearchId));
        }
    }
}
