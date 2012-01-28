using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.SelectLists
{
    public class JigsawSelectLists
    {
        public static IList<SelectListItem> GetIndustryList()
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

        public static IList<SelectListItem> GetDepartmentList()
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

        public static IList<SelectListItem> GetContactLevelsList()
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

        public static IList<SelectListItem> GetMetroAreas()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified>", Value = string.Empty },
                new SelectListItem { Text = "Atlanta" },
                new SelectListItem { Text = "Baltimore" },
                new SelectListItem { Text = "Washington" },
                new SelectListItem { Text = "Boston" },
                new SelectListItem { Text = "Chicago" },
                new SelectListItem { Text = "Cleveland" },
                new SelectListItem { Text = "Dallas" },
                new SelectListItem { Text = "Denver" },
                new SelectListItem { Text = "Detroit" },
                new SelectListItem { Text = "Houston" },
                new SelectListItem { Text = "Los Angeles" },
                new SelectListItem { Text = "Miami" },
                new SelectListItem { Text = "Minneapolis" },
                new SelectListItem { Text = "St. Paul" },
                new SelectListItem { Text = "New York" },
                new SelectListItem { Text = "Philadelphia" },
                new SelectListItem { Text = "Phoenix" },
                new SelectListItem { Text = "Portland" },
                new SelectListItem { Text = "Saint Louis" },
                new SelectListItem { Text = "Salt Lake City" },
                new SelectListItem { Text = "San Diego" },
                new SelectListItem { Text = "San Francisco" },
                new SelectListItem { Text = "Seattle" },
                new SelectListItem { Text = "Calgary" },
                new SelectListItem { Text = "Edmonton" },
                new SelectListItem { Text = "Montreal" },
                new SelectListItem { Text = "Ottawa" },
                new SelectListItem { Text = "Quebec" },
                new SelectListItem { Text = "Toronto" },
                new SelectListItem { Text = "Vancouver" },
                new SelectListItem { Text = "Victoria" },
                new SelectListItem { Text = "Winnipeg" }
            };
        }

        public static IList<SelectListItem> GetEmployeeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified>", Value = string.Empty },
                new SelectListItem { Text = "0-25" },
                new SelectListItem { Text = "25-100" },
                new SelectListItem { Text = "100-250" },
                new SelectListItem { Text = "250-1k" },
                new SelectListItem { Text = "1k-10k" },
                new SelectListItem { Text = "10k-50k" },
                new SelectListItem { Text = "50k-100k" },
                new SelectListItem { Text = "100k+", Value="100k" }
            };
        }

        public static IList<SelectListItem> GetRevenues()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified>", Value = string.Empty },
                new SelectListItem { Text = "0-1m" },
                new SelectListItem { Text = "1m-10m" },
                new SelectListItem { Text = "10m-50m" },
                new SelectListItem { Text = "50m-100m" },
                new SelectListItem { Text = "100m-250m" },
                new SelectListItem { Text = "250m-500m" },
                new SelectListItem { Text = "500m-1b" },
                new SelectListItem { Text = "1b+", Value = "1b" }
            };
        }

        public static IList<SelectListItem> GetOwnershipTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified>", Value = string.Empty },
                new SelectListItem { Text = "public" },
                new SelectListItem { Text = "private" },
                new SelectListItem { Text = "government" },
                new SelectListItem { Text = "other" }
            };
        }

        public static IList<SelectListItem> GetFortuneRanks()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "<None Specified>", Value = string.Empty },
                new SelectListItem { Text = "500" },
                new SelectListItem { Text = "1000" }
            };
        }
    }
}