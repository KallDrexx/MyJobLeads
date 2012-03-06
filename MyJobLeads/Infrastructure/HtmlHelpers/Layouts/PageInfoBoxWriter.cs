using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class PageInfoBoxWriter : IDisposable
    {
        protected ViewContext _viewContext;
        protected bool _includesSeparator;

        public PageInfoBoxWriter(ViewContext context, bool includeSeparator)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;
            _includesSeparator = includeSeparator;

            // Write the html
            _viewContext.Writer.Write("<div class=\"grid1 floatLeft\">");

            if (_includesSeparator) _viewContext.Writer.Write("<div class=\"lineSeperater\">");

            _viewContext.Writer.Write("<div class=\"pageInfoBox\">");

            return;
        }

        public void Dispose()
        {
            _viewContext.Writer.Write("<div class=\"clear\"></div></div></div>");
            if (_includesSeparator) _viewContext.Writer.Write("</div>");
        }
    }
}