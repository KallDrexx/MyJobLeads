using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class FieldValueDisplayWriter : IDisposable
    {
        protected ViewContext _context;

        public FieldValueDisplayWriter(ViewContext context, HtmlString label)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
            _context.Writer.Write("<p class=\"greyHighlight\">" + label.ToHtmlString() + "<span class=\"setTask\">");
        }

        public void Dispose()
        {
            _context.Writer.Write("</span></p>");
        }
    }
}