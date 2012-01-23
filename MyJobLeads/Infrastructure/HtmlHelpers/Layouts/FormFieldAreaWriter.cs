using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class FormFieldAreaWriter : IDisposable
    {
        protected ViewContext _context;

        public FormFieldAreaWriter(ViewContext context, HtmlString fieldLabel)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
            _context.Writer.Write("<div class=\"floatLeft\"><p class=\"greyHighlight\">");
            _context.Writer.Write(fieldLabel);
            _context.Writer.Write("</p><div class=\"infoSpan\">");
        }

        public void Dispose()
        {
            _context.Writer.Write("</div></div>");
        }
    }
}