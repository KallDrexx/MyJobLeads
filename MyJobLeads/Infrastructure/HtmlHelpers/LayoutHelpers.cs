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

        public static OuterRowWriter OuterRow(this HtmlHelper html)
        {
            return new OuterRowWriter(html.ViewContext);
        }

        public static FormFieldWriter FormField(this HtmlHelper html, string field, HtmlString editor)
        {
            return new FormFieldWriter(html.ViewContext, new HtmlString(field), editor);
        }

        public static FormFieldWriter FormField(this HtmlHelper html, HtmlString field, HtmlString editor)
        {
            return new FormFieldWriter(html.ViewContext, field, editor);
        }

        public static FormButtonAreaWriter FormButtonArea(this HtmlHelper html)
        {
            return new FormButtonAreaWriter(html.ViewContext);
        }
    }
}