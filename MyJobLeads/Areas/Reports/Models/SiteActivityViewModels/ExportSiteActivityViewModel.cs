using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyJobLeads.Areas.Reports.Models.SiteActivityViewModels
{
    public class ExportSiteActivityViewModel
    {
        [Required(ErrorMessage = "A valid start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "A valid end date is required")]
        public DateTime EndDate { get; set; }

        public string IgnoredUserList { get; set; }

        public bool SortByDescending { get; set; }
    }
}