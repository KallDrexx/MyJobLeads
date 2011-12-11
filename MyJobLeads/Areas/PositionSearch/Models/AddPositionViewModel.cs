using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Enums;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.PositionSearch.Models
{
    public class AddPositionViewModel
    {
        public string PositionId { get; set; }
        public string PositionTitle { get; set; }
        public string CompanyName { get; set; }
        public ExternalDataSource DataSource { get; set; }
        public int SelectedCompany { get; set; }
        public bool CreateNewCompany { get; set; }

        public IList<SelectListItem> CompanyList { get; set; }

        public void SetCompanyList(IList<Company> companies)
        {
            CompanyList = companies.OrderBy(x => x.Name)
                                   .Select(x => new SelectListItem
                                   {
                                       Value = x.Id.ToString(),
                                       Text = x.Name
                                   })
                                   .ToList();
            CompanyList.Insert(0, new SelectListItem { Value = "0", Text = "<None Selected>" });
        }
    }
}