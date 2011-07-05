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
    using System.Web.Mvc.Html;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Task/Details.cshtml")]
    public class Details : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.Entities.Task>
    {
        public Details()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
  
    ViewBag.Title = "Task: " + Model.Name;
    Layout = "~/Views/Shared/_Layout.cshtml";

    // Setup the sidebar
    if (Model.ContactId != null) 
    {
        ViewBag.Sidebar = new HtmlString(Html.Partial(MVC.Contact.Views._ContactSidebarDisplay, Model.Contact).ToString() +
                                            Html.Partial(MVC.Company.Views._CompanySidebarDisplay, Model.Company).ToString());
    }
    else { ViewBag.Sidebar = Html.Partial(MVC.Company.Views._CompanySidebarDisplay, Model.Company); }


            
            #line default
            #line hidden
WriteLiteral("\r\n<h2>");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</h2> <br />                                                                     " +
"                 \r\n\r\n<p>\r\n");


            
            #line 19 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
     if (Model.ContactId != null) 
    { 

            
            #line default
            #line hidden
WriteLiteral("        ");

WriteLiteral("This task is associated with ");


            
            #line 21 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                                  Write(Html.ActionLink(Model.Contact.Name, MVC.Contact.Details((int)Model.ContactId)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 22 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        ");

WriteLiteral("This task is associated with ");


            
            #line 25 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                                  Write(Html.ActionLink(Model.Company.Name, MVC.Company.Details((int)Model.CompanyId)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 26 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n    <p>\r\n        Category: ");


            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
             Write(Model.Category);

            
            #line default
            #line hidden

            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                                 WriteLiteral(" <br />\r\n        Task Due Date: ");

            
            #line default
            #line hidden
            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                        if (Model.TaskDate.HasValue) { 
            
            #line default
            #line hidden
            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                                                  Write(Model.TaskDate.Value.ToString());

            
            #line default
            #line hidden
            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                                                                                        } else {
            
            #line default
            #line hidden
WriteLiteral(" ");

WriteLiteral("&lt;No Date Set&gt;");

WriteLiteral(" ");


            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                                                                                                                                  }
            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n\r\n");


            
            #line 32 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
         if (Model.CompletionDate == null) 
        { 

            
            #line default
            #line hidden
WriteLiteral("            <b>Not Completed</b>\r\n");


            
            #line 35 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
        }
        else 
        { 

            
            #line default
            #line hidden
WriteLiteral("            ");

WriteLiteral("Completed on: ");


            
            #line 38 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
                       Write(Model.CompletionDate.Value.ToString());

            
            #line default
            #line hidden
WriteLiteral(" \r\n");


            
            #line 39 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <br />\r\n\r\n        <table>\r\n            <tr>\r\n                <td valign=\"" +
"top\">Notes:  &nbsp;</td>\r\n                <td>");


            
            #line 45 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
               Write(Html.Raw(Html.Encode(Model.Notes).Replace(Environment.NewLine, "<br />")));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n            </tr>\r\n        </table>\r\n    </p>                             " +
"                          \r\n</p>\r\n\r\n");


            
            #line 51 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Details.cshtml"
Write(Html.ActionLink("Edit Task Details", MVC.Task.Edit(Model.Id)));

            
            #line default
            #line hidden
WriteLiteral(" &nbsp; \r\n");


        }
    }
}
#pragma warning restore 1591