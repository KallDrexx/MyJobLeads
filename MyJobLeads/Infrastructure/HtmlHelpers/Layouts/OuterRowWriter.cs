using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class OuterRowWriter : IDisposable
    {
        protected ViewContext _viewContext;

        public OuterRowWriter(ViewContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;
            _viewContext.Writer.Write("<div class=\"grid3 marginBottom_10 marginAuto floatLeft\">");
        }

        public void Dispose()
        {
            _viewContext.Writer.Write("</div>");
        }
    }
}