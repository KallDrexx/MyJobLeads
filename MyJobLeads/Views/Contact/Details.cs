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

namespace MyJobLeads.Views.Contact
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
    
    #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
    using MyJobLeads.DomainModel.Entities;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Contact/Details.cshtml")]
    public class Details : System.Web.Mvc.WebViewPage<Contact>
    {
        public Details()
        {
        }
        public override void Execute()
        {


WriteLiteral("\r\n");


            
            #line 4 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
  
    ViewBag.Title = "Contact: " + Model.Name;
    Layout = "~/Views/Shared/_Layout.cshtml";
    ViewBag.Sidebar = Html.Partial(MVC.Company.Views._CompanySidebarDisplay, Model.Company);

    string taskString = Model.Tasks.Count == 1 ? "task" : "tasks";


            
            #line default
            #line hidden
WriteLiteral("\r\n<h2>");


            
            #line 12 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("\'s Details</h2>\r\n\r\n<p>\r\n    ");


            
            #line 15 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral(" is a contact for ");


            
            #line 15 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                            Write(Html.ActionLink(Model.Company.Name, MVC.Company.Details(Model.Company.Id)));

            
            #line default
            #line hidden
WriteLiteral(" <br /><br />\r\n    Direct Phone: ");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
             Write(Model.DirectPhone);

            
            #line default
            #line hidden

            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                      if (!string.IsNullOrWhiteSpace(Model.Extension)) {
            
            #line default
            #line hidden
WriteLiteral(" ");

WriteLiteral("Ext: ");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                                                                               Write(Model.Extension);

            
            #line default
            #line hidden
WriteLiteral(" ");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                                                                                                            }
            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n    Mobile Phone: ");


            
            #line 17 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
             Write(Model.MobilePhone);

            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n    Email Address: <a href=\"mailto:");


            
            #line 18 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                              Write(Model.Email);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 18 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                            Write(Model.Email);

            
            #line default
            #line hidden
WriteLiteral("</a> <br />\r\n    Assistant: ");


            
            #line 19 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
          Write(Model.Assistant);

            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n    Referred By: ");


            
            #line 20 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
            Write(Model.ReferredBy);

            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n    Notes: ");


            
            #line 21 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
      Write(Html.Raw(Html.Encode(Model.Notes).Replace(Environment.NewLine, "<br />")));

            
            #line default
            #line hidden
WriteLiteral("\r\n</p>\r\n\r\n");


            
            #line 24 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
Write(Html.ActionLink("Edit Contact Details", MVC.Contact.Edit(Model.Id)));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<br /><br />\r\n<h2>Tasks</h2>\r\nThis contact has ");


            
            #line 28 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
            Write(Model.Tasks.Count);

            
            #line default
            #line hidden
WriteLiteral(" ");


            
            #line 28 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                               Write(taskString);

            
            #line default
            #line hidden
WriteLiteral(" entered \r\n(");


            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
Write(Html.ActionLink("Add", MVC.Task.Add((int)Model.CompanyId, Model.Id)));

            
            #line default
            #line hidden
WriteLiteral(")\r\n\r\n<ul>\r\n");


            
            #line 32 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
     foreach (Task task in Model.Tasks)
    {

            
            #line default
            #line hidden
WriteLiteral("        <li>\r\n            ");


            
            #line 35 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
       Write(Html.ActionLink(task.Name, MVC.Task.Details(task.Id)));

            
            #line default
            #line hidden
WriteLiteral(" - \r\n\r\n");


            
            #line 37 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
             if (task.CompletionDate != null) {
            
            #line default
            #line hidden
WriteLiteral(" ");

WriteLiteral("Completed on ");


            
            #line 37 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                                              Write(task.CompletionDate.Value.ToString());

            
            #line default
            #line hidden
WriteLiteral(" ");


            
            #line 37 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                                                                                                }
            else {
            
            #line default
            #line hidden
WriteLiteral(" <b>Not Completed</b> ");


            
            #line 38 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                        }

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 40 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
             if (task.TaskDate != null && task.CompletionDate == null) {
            
            #line default
            #line hidden
WriteLiteral(" ");

WriteLiteral(" - Due: ");


            
            #line 40 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                                                                  Write(task.TaskDate.Value.ToString());

            
            #line default
            #line hidden
WriteLiteral(" ");


            
            #line 40 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
                                                                                                                              }

            
            #line default
            #line hidden
WriteLiteral("        </li>\r\n");


            
            #line 42 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Details.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</ul>");


        }
    }
}
#pragma warning restore 1591