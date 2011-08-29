using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers
{
    public static class StringHtmlHelpers
    {
        public static string ShortString(this HtmlHelper html, string longString, int length)
        {
            if (string.IsNullOrWhiteSpace(longString))
                return string.Empty;

            if (longString.Length < length)
                return longString;

            return longString.Substring(0, length) + "...";
        }
    }
}