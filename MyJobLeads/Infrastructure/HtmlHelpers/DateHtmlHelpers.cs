using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers
{
    public static class DateHtmlHelpers
    {
        public static string GetDateString(this HtmlHelper html, DateTime? date)
        {
            if (date == null || date.HasValue == false)
                return string.Empty;

            // Determine if the date is today, yesterday, or tomorrow for friendly strings
            if (date.Value.Date == DateTime.Today - new TimeSpan(1, 0, 0, 0))
                return "Yesterday";

            if (date.Value.Date == DateTime.Today)
                return "Today";

            if (date.Value.Date == DateTime.Today + new TimeSpan(1, 0, 0, 0))
                return "Tomorrow";

            // Otherwise just return the date string
            return date.Value.ToShortDateString();
        }
    }
}