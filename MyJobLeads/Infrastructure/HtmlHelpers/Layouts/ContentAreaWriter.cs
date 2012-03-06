using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class ContentAreaWriter : IDisposable
    {
        protected ViewContext _context;

        public ContentAreaWriter(ViewContext context, bool withInfospan = false)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
            _context.Writer.Write(string.Format("<div class=\"floatLeft {0}\">", withInfospan ? "infoSpan" : string.Empty));
        }

        public void Dispose()
        {
            _context.Writer.Write("</div>");
        }
    }
}