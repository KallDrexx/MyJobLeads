using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.SelectLists;

namespace MyJobLeads.Areas.ContactSearch.Models
{
    public class JigsawSearchParametersViewModel
    {
        public int Page { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Industry { get; set; }
        public string Levels { get; set; }
        public string Departments { get; set; }
        public int MergedContactId { get; set; }

        public IList<SelectListItem> LevelsList { get; set; }
        public IList<SelectListItem> IndustryList { get; set; }
        public IList<SelectListItem> DepartmentList { get; set; }

        public JigsawSearchParametersViewModel()
        {
            LevelsList = JigsawSelectLists.GetContactLevelsList();
            DepartmentList = JigsawSelectLists.GetDepartmentList();
            IndustryList = JigsawSelectLists.GetIndustryList();
        }

        public string GetSearchUrl()
        {
            return string.Format("FirstName={0}&LastName={1}&Company={2}&Title={3}&Industry={4}&Levels={5}&Departments={6}&MergedContactId={7}",
                                    HttpUtility.UrlEncode(FirstName),
                                    HttpUtility.UrlEncode(LastName),
                                    HttpUtility.UrlEncode(Company),
                                    HttpUtility.UrlEncode(Title),
                                    HttpUtility.UrlEncode(Industry),
                                    HttpUtility.UrlEncode(Levels),
                                    HttpUtility.UrlEncode(Departments),
                                    MergedContactId);
        }
    }
}