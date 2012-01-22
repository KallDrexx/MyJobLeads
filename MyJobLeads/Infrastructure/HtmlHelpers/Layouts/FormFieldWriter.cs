using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Infrastructure.HtmlHelpers.Layouts
{
    public class FormFieldWriter
    {
        protected ViewContext _viewContext;

        public FormFieldWriter(ViewContext context, HtmlString fieldName, HtmlString fieldEditor)
        {
            if (context == null)
                throw new ArgumentNullException("fieldName");

            _viewContext = context;
            
            // Form html
            var html = string.Format("<div class=\"floatLeft\"><p class=\"greyHighlight\">{0}</p><div class=\"infoSpan\">{1}</div></div>", fieldName, fieldEditor);
            _viewContext.Writer.Write(html);
        }
    }
}