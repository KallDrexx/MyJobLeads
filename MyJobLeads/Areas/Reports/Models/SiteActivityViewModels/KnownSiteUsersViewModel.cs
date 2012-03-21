using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Areas.Reports.Models.SiteActivityViewModels
{
    public class KnownSiteUsersViewModel
    {
        public List<SelectListItem> KnownUsersList { get; set; }
        public string SelectedUserIp { get; set; }
    }
}