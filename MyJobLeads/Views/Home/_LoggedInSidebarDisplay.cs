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

namespace MyJobLeads.Views.Home
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
    
    #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_LoggedInSidebarDisplay.cshtml"
    using MyJobLeads.Controllers;
    
    #line default
    #line hidden
    using MyJobLeads.Infrastructure.HtmlHelpers;
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.1.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/_LoggedInSidebarDisplay.cshtml")]
    public class _LoggedInSidebarDisplay : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.Users.UserSidebarViewModel>
    {
        public _LoggedInSidebarDisplay()
        {
        }
        public override void Execute()
        {


WriteLiteral("\r\n<h2 id=\"accountName\">");


            
            #line 4 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_LoggedInSidebarDisplay.cshtml"
                Write(Model.DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n<div class=\"accountEdit\"><a href=\"#\">Edit Your Account</a></div>\r\n\r\n<div i" +
"d=\"leftHandNav\">\r\n    <ul>\r\n        <li>");


            
            #line 9 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_LoggedInSidebarDisplay.cshtml"
       Write(Html.ActionLink("Your Tasks", MVC.Task.Index(), new { @class = Model.ActiveLink == ActiveSidebarLink.Tasks? "active" : "" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n        <li>");


            
            #line 10 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_LoggedInSidebarDisplay.cshtml"
       Write(Html.ActionLink("Your Companies", MVC.Company.List(), new { @class = Model.ActiveLink == ActiveSidebarLink.Companies? "active" : "" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n        <li>");


            
            #line 11 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_LoggedInSidebarDisplay.cshtml"
       Write(Html.ActionLink("About", MVC.Home.About(), new { @class = Model.ActiveLink == ActiveSidebarLink.About? "active" : "" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n    </ul>\r\n</div>");


        }
    }
}
#pragma warning restore 1591
