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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Task/_OpenTaskListItem.cshtml")]
    public class _OpenTaskListItem : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.Entities.Task>
    {
        public _OpenTaskListItem()
        {
        }
        public override void Execute()
        {

WriteLiteral("           \r\n");


            
            #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\_OpenTaskListItem.cshtml"
  
    string linkString = string.Concat(Model.Name, " for ", Model.Company.Name);


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\_OpenTaskListItem.cshtml"
Write(Html.ActionLink(Model.Name, MVC.Task.Details(Model.Id)));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<ul>\r\n    <li>For ");


            
            #line 10 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\_OpenTaskListItem.cshtml"
       Write(Html.ActionLink(Model.Company.Name, MVC.Company.Details(Model.Company.Id)));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n\r\n\r\n");


            
            #line 13 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\_OpenTaskListItem.cshtml"
     if (Model.TaskDate != null && Model.TaskDate.HasValue)
    {

            
            #line default
            #line hidden
WriteLiteral("        <li>Due: ");


            
            #line 15 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\_OpenTaskListItem.cshtml"
            Write(Model.TaskDate.ToString());

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Task\_OpenTaskListItem.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n</ul>");


        }
    }
}
#pragma warning restore 1591
