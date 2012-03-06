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
            using (new FormFieldAreaWriter(_viewContext, fieldName))
            {
                _viewContext.Writer.Write(fieldEditor);
            }
        }
    }
}