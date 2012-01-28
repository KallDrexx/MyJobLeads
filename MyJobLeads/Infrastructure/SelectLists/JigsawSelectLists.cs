using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.SelectLists
{
    public class JigsawSelectLists
    {
        public static List<SelectListItem> GetIndustryList()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Text = "<None Specified>", Value = string.Empty },
                new SelectListItem { Text = "Agriculture & Mining", Value = "1010000" },
                new SelectListItem { Text = "Business Services", Value = "1020000" },
                new SelectListItem { Text = "Computers & Electronics", Value = "1030000" },
                new SelectListItem { Text = "Education", Value = "1050000" },
                new SelectListItem { Text = "Energy & Utilities", Value = "1060000" },
                new SelectListItem { Text = "Financial Services", Value = "1070000" },
                new SelectListItem { Text = "Government", Value = "1090000" },
                new SelectListItem { Text = "Healthcare, Pharmaceuticals, & Biotech", Value = "1100000" },
                new SelectListItem { Text = "Manufacturing", Value = "1110000" },
                new SelectListItem { Text = "Media & Entertainment", Value = "1120000" },
                new SelectListItem { Text = "Non-Profit", Value = "1130000" },
                new SelectListItem { Text = "Real Estate & Construction", Value = "1140000" },
                new SelectListItem { Text = "Retail", Value = "1160000" },
                new SelectListItem { Text = "Software & Internet", Value = "1180000" },
                new SelectListItem { Text = "Telecommunications", Value = "1190000" },
                new SelectListItem { Text = "Transportation & Storage", Value = "1200000" },
                new SelectListItem { Text = "Travel, Recreation, and Leisure", Value = "1210000" },
                new SelectListItem { Text = "Wholesale & Distribution", Value = "1220000" },
                new SelectListItem { Text = "Consumer Services", Value = "1230000" },
                new SelectListItem { Text = "Other", Value = "1240000" }
            };
        }

        public static List<SelectListItem> GetDepartmentList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified", Value = string.Empty },
                new SelectListItem { Text = "Sales", Value = "sales" },
                new SelectListItem { Text = "Marketing", Value = "marketing" },
                new SelectListItem { Text = "Finance", Value = "finance" },
                new SelectListItem { Text = "HR", Value = "HR" },
                new SelectListItem { Text = "Support", Value = "support" },
                new SelectListItem { Text = "Engineering", Value = "engineering" },
                new SelectListItem { Text = "Operations", Value = "operations" },
                new SelectListItem { Text = "IT", Value = "IT" },
                new SelectListItem { Text = "Other", Value = "other" }
            };
        }

        public static List<SelectListItem> GetContactLevelsList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified", Value = string.Empty },
                new SelectListItem { Text = "C-level", Value = "C-Level" },
                new SelectListItem { Text = "VP", Value = "VP" },
                new SelectListItem { Text = "Director", Value = "Director" },
                new SelectListItem { Text = "Manager", Value = "Manager" },
                new SelectListItem { Text = "Staff", Value = "Staff" }
            };
        }
    }
}