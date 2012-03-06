using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class FormButtonAreaWriter : IDisposable
    {
        protected ViewContext _viewContext;

        public FormButtonAreaWriter(ViewContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;
            _viewContext.Writer.Write("<div class=\"submitBTN\">");
        }

        public void Dispose()
        {
            _viewContext.Writer.Write("</div>");
        }
    }
}