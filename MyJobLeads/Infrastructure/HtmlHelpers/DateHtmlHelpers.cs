using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers
{
    public static class DateHtmlHelpers
    {
        public static string GetDueDateString(this HtmlHelper html, DateTime? date)
        {
            if (date == null || date.HasValue == false)
                return string.Empty;

            // Determine if the date is today, yesterday, or tomorrow for friendly strings
            if (date.Value.Date == DateTime.Today - new TimeSpan(1, 0, 0, 0))
                return "Due Yesterday";

            if (date.Value.Date == DateTime.Today)
                return "Due Today";

            if (date.Value.Date == DateTime.Today + new TimeSpan(1, 0, 0, 0))
                return "Due Tomorrow";

            // Otherwise just return the date string
            return "Due " + date.Value.ToString("M");
        }
    }
}