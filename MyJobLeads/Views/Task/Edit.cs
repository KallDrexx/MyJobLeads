﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyJobLeads.Views.Task
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    using MyJobLeads.ViewModels.Companies;
    
    #line default
    #line hidden
    
    #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    using MyJobLeads.ViewModels.Tasks;
    
    #line default
    #line hidden
    
    #line 4 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    using Telerik.Web.Mvc.UI;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.1.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Task/Edit.cshtml")]
    public class Edit : System.Web.Mvc.WebViewPage<EditTaskViewModel>
    {
        public Edit()
        {
        }
        public override void Execute()
        {




WriteLiteral("\r\n");


            
            #line 6 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
  
    string editActionLabel;
    
    if (Model.Id == 0)
    {
        ViewBag.Title = "Add Your Task";
        editActionLabel = "Add Your Task";
    }
    else
    {
        ViewBag.Title = "Edit Your Task";
        editActionLabel = "Edit Your Task";
    }


            
            #line default
            #line hidden
WriteLiteral("\r\n");



WriteLiteral("\r\n\r\n");


            
            #line 37 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
 using (Html.BeginForm(MVC.Task.Edit(), FormMethod.Post, new { @class = "display-form" }))
{

            
            #line default
            #line hidden
WriteLiteral("    <h2>");


            
            #line 39 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
   Write(editActionLabel);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");


            
            #line 40 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    
    
            
            #line default
            #line hidden
            
            #line 41 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
            
            #line 41 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                              
    
            
            #line default
            #line hidden
            
            #line 42 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
Write(Html.HiddenFor(x => x.AssociatedCompanyId));

            
            #line default
            #line hidden
            
            #line 42 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                               


            
            #line default
            #line hidden
WriteLiteral("    <p>");


            
            #line 44 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
  Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");


            
            #line 45 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"form-row\">\r\n        <span class=\"form-label\">Subject:</span>\r\n   " +
"     <span class=\"form-field\">");


            
            #line 48 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                            Write(Html.TextBoxFor(x => x.Name, new { size = 30 }));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n");


            
            #line 50 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"form-row\">\r\n        <span class=\"form-label\">Category:</span>\r\n  " +
"      <span class=\"form-field\">");


            
            #line 53 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                            Write(Html.DropDownListFor(x => x.Category, new SelectList(Model.AvailableCategoryList, Model.Category)));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n");


            
            #line 55 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <br />");



WriteLiteral("<br />\r\n");


            
            #line 57 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"form-row\">\r\n        <span class=\"form-label\">Associate With:</spa" +
"n>\r\n        <span class=\"form-field\">");


            
            #line 60 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                            Write(Html.DropDownListFor(x => x.AssociatedContactId, Model.CompanyContactList, new { id = "contactlist" }));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n");


            
            #line 62 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    
    

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"form-row\">\r\n        <span class=\"form-label\">Task Due Date:</span" +
">\r\n        <span class=\"form-field\">");


            
            #line 66 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                            Write(Html.Telerik().DatePickerFor(x => x.TaskDate));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n");


            
            #line 68 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    
    
            
            #line default
            #line hidden

            
            #line 69 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                                             
    
            
            #line default
            #line hidden
            
            #line 70 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
     if (Model.Id != 0)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div class=\"form-row\">\r\n            <span class=\"form-label\">Task Complet" +
"ed?</span>\r\n            <span class=\"form-field\">");


            
            #line 74 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                Write(Html.CheckBoxFor(x => x.Completed));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n        </div>\r\n");


            
            #line 76 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    }
            
            #line default
            #line hidden
            
            #line 76 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
     
    

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"form-row\">\r\n        <span class=\"form-label\">Notes:</span>\r\n     " +
"   <span class=\"form-field\">");


            
            #line 80 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                            Write(Html.TextAreaFor(x => x.Notes, 10, 75, null));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n");


            
            #line 82 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"form-row\">\r\n        <span class=\"form-label\">&nbsp;</span>\r\n     " +
"   <span class=\"form-field\">\r\n            <input type=\"submit\" value=\"Save\" /> &" +
"nbsp; &nbsp;\r\n\r\n");


            
            #line 88 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
             if (Model.Id != 0)
            { 
                
            
            #line default
            #line hidden
            
            #line 90 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.ActionLink("Back To Task", MVC.Task.Details(Model.Id), new { @class = "details-link" }));

            
            #line default
            #line hidden
            
            #line 90 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                                                                             ;
            }
            else if (Model.AssociatedCompanyId != 0)
            { 
                
            
            #line default
            #line hidden
            
            #line 94 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.ActionLink("Back To Company", MVC.Company.Details(Model.AssociatedCompanyId), new { @class = "details-link" }));

            
            #line default
            #line hidden
            
            #line 94 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                                                                                                     
            }    

            
            #line default
            #line hidden
WriteLiteral("        </span>\r\n    </div>        \r\n");


            
            #line 98 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<hr />\r\n\r\n");


            
            #line 102 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
 if (Model.AssociatedContactId > 0)
{
    Html.RenderPartial(MVC.Contact.Views._ContactSidebarDisplay, Model.Contact);
}
else
{
    Html.RenderPartial(MVC.Company.Views._CompanySidebarDisplay, new CompanyDisplayViewModel(Model.Company));
}
            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
