using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class InnerColumnWriter : IDisposable
    {
        protected ViewContext _context;

        public InnerColumnWriter(ViewContext context, bool floatLeft)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
            if (floatLeft)
                _context.Writer.Write("<div class=\"grid4 floatLeft\">");
            else
                _context.Writer.Write("<div class=\"grid4 floatRight\">");
        }

        public void Dispose()
        {
            _context.Writer.Write("</div>");
        }
    }
}