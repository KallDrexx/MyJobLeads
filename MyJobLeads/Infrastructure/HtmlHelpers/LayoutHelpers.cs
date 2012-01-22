using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.Infrastructure.HtmlHelpers.Layouts;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers
{
    public static class LayoutHelpers
    {
        public static PageInfoBoxWriter PageInfoBox(this HtmlHelper html, bool includeSeparator)
        {
            return new PageInfoBoxWriter(html.ViewContext, includeSeparator);
        }
    }
}