using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.SelectLists;

namespace MyJobLeads.Areas.CompanySearch.Models.Jigsaw
{
    public class CompanySearchQueryViewModel
    {
        public int Page { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string MetroArea { get; set; }
        public string State { get; set; }
        public string Employees { get; set; }
        public string Revenue { get; set; }
        public string Ownership { get; set; }

        public IList<SelectListItem> IndustryList { get; set; }
        public IList<SelectListItem> MetroAreaList { get; set; }
        public IList<SelectListItem> StateList { get; set; }
        public IList<SelectListItem> EmployeeList { get; set; }
        public IList<SelectListItem> RevenueList { get; set; }
        public IList<SelectListItem> OwnershipTypes { get; set; }

        public CompanySearchQueryViewModel()
        {
            IndustryList = JigsawSelectLists.GetIndustryList();
            MetroAreaList = JigsawSelectLists.GetMetroAreas();
            StateList = GeographicSelectLists.GetStateList();
            EmployeeList = JigsawSelectLists.GetEmployeeList();
            RevenueList = JigsawSelectLists.GetRevenues();
            OwnershipTypes = JigsawSelectLists.GetOwnershipTypes();
        }

        public string GetSearchUrl()
        {
            return string.Format("CompanyName={0}&Industry={1}&MetroArea={2}&State={3}&Employees={4}&Revenue={5}&Ownership={6}",
                HttpUtility.UrlEncode(CompanyName),
                HttpUtility.UrlEncode(Industry),
                HttpUtility.UrlEncode(MetroArea),
                HttpUtility.UrlEncode(State),
                HttpUtility.UrlEncode(Employees),
                HttpUtility.UrlEncode(Revenue),
                HttpUtility.UrlEncode(Ownership));
        }
    }
}