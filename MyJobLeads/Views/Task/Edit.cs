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
    
    #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    using MyJobLeads.ViewModels.Tasks;
    
    #line default
    #line hidden
    
    #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
    using Telerik.Web.Mvc.UI;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Task/Edit.cshtml")]
    public class Edit : System.Web.Mvc.WebViewPage<EditTaskViewModel>
    {
        public Edit()
        {
        }
        public override void Execute()
        {



WriteLiteral("\r\n");


            
            #line 5 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";

    // Setup the sidebar
    string sidebar = "<div id=\"contactsummary\">";
    
    if (Model.Contact != null)
    {
        sidebar += Html.Partial(MVC.Contact.Views._ContactSidebarDisplay, Model.Contact).ToString();
    }

    sidebar = string.Concat(sidebar, "</div>", Html.Partial(MVC.Company.Views._CompanySidebarDisplay, Model.Company));
    ViewBag.Sidebar = new HtmlString(sidebar);


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 20 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
 if (Model.Id == 0)
{
    ViewBag.Title = "Add A Task";

            
            #line default
            #line hidden
WriteLiteral("    <h2>Add A New Task</h2>\r\n");



WriteLiteral("    <div class=\"ScreenHint\">This screen allows you to create a new task.</div>\r\n");


            
            #line 25 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
}
else
{
    ViewBag.Title = "Editing " + @Model.Name;

            
            #line default
            #line hidden
WriteLiteral("    <h2>Editing ");


            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");



WriteLiteral("    <div class=\"ScreenHint\">This screen allows you to edit the ");


            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                          Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral(" task.</div>\r\n");


            
            #line 31 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<br />\r\n\r\n");


            
            #line 35 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
  Html.Telerik().ScriptRegistrar().OnDocumentReady(
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

            
            #line default
            #line hidden

WriteLiteralTo(@__razor_template_writer, "    ");

WriteLiteralTo(@__razor_template_writer, @"
        // Upon contact selection change, update the contact sidebar summary
        $('#contactlist').change(function() {
            $.ajax({
                url: '/Contact/GetSummary/' + $(this).val(),
                success: function(html) {
                    $('#contactsummary').html(html);
                }
            });
        });
    ");


            
            #line 46 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
         }));


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 49 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
 using (Html.BeginForm(MVC.Task.Edit()))
{
    
            
            #line default
            #line hidden
            
            #line 51 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
            
            #line 51 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                              
    
            
            #line default
            #line hidden
            
            #line 52 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
Write(Html.HiddenFor(x => x.AssociatedCompanyId));

            
            #line default
            #line hidden
            
            #line 52 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                               


            
            #line default
            #line hidden
WriteLiteral("    <table>\r\n        <tr>\r\n            <td align=\"right\">Category:</td>\r\n        " +
"    <td>\r\n                ");


            
            #line 58 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.DropDownListFor(x => x.Category, new SelectList(Model.AvailableCategoryList, Model.Category)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\"" +
">Subject:</td>\r\n            <td>");


            
            #line 64 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.TextBoxFor(x => x.Name, new { size = 30 }));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Contact:</td>" +
"\r\n            <td>\r\n                ");


            
            #line 70 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.DropDownListFor(x => x.AssociatedContactId, Model.CompanyContactList, new { id = "contactlist" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\"" +
">Task Due Date:</td>\r\n            <td>\r\n                ");


            
            #line 77 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.Telerik().DateTimePickerFor(x => x.TaskDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n\r\n        ");



WriteLiteral("\r\n");


            
            #line 82 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
         if (Model.Id != 0)
        {

            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <td align=\"right\">Task Completed?</td>\r\n       " +
"         <td>");


            
            #line 86 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
               Write(Html.CheckBoxFor(x => x.Completed));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n            </tr>\r\n");


            
            #line 88 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        \r\n        <tr>\r\n            <td align=\"right\" valign=\"top\">Notes:</td>\r\n " +
"           <td>\r\n                ");


            
            #line 93 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
           Write(Html.TextAreaFor(x => x.Notes, 10, 75, null));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        \r\n        <tr>\r\n            <td></td>" +
"\r\n            <td>\r\n                <input type=\"submit\" value=\"Save\" /> &nbsp; " +
"&nbsp; \r\n\r\n");


            
            #line 102 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                 if (Model.Id != 0) { 
            
            #line default
            #line hidden
            
            #line 102 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                 Write(Html.ActionLink("Back To Task", MVC.Task.Details(Model.Id)));

            
            #line default
            #line hidden
            
            #line 102 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                                                                  ; }
                else if (Model.AssociatedCompanyId != 0) { 
            
            #line default
            #line hidden
            
            #line 103 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                      Write(Html.ActionLink("Back To Company", MVC.Company.Details(Model.AssociatedCompanyId)));

            
            #line default
            #line hidden
            
            #line 103 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
                                                                                                                                               }                

            
            #line default
            #line hidden
WriteLiteral("            </td>\r\n        </tr>\r\n    </table>\r\n");


            
            #line 107 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\Edit.cshtml"
        
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");


        }
    }
}
#pragma warning restore 1591
