using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers
{
    public static class NumericHtmlDisplayHelpers
    {
        public static string DisplayPercentageString(this HtmlHelper html, int numerator, int denominator)
        {
            int percentage = (int)(DisplayPercentageDecimal(html, numerator, denominator) * 100);
            return percentage.ToString() + "%"; 
        }

        public static decimal DisplayPercentageDecimal(this HtmlHelper helper, int numerator, int denominator)
        {
            if (denominator == 0)
                return 0;

            return Math.Round((decimal)numerator / (decimal)denominator, 2);
        }
    }
}