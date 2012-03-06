using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.CompanySearch.Models.Jigsaw
{
    public class AddCompanyViewModel
    {
        public int JigsawId { get; set; }
        public string JigsawCompanyName { get; set; }
        public bool CreateNewCompany { get; set; }
        public int ExistingCompanyId { get; set; }
        public IList<SelectListItem> ExistingCompanyList { get; set; }

        public void CreateCompanyList(JobSearch jobsearch)
        {
            ExistingCompanyList = jobsearch.Companies
                                           .OrderBy(x => x.Name)
                                           .Select(x => new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString()
                                           })
                                           .ToList();

            ExistingCompanyList.Insert(0, new SelectListItem { Text = "<None Selected>", Value = "0" });
        }
    }
}