﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
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
    
    #line 2 "..\..\Views\Home\_AnonymousUserSidebarDisplay.cshtml"
    using MyJobLeads.Controllers;
    
    #line default
    #line hidden
    using MyJobLeads.Infrastructure.HtmlHelpers;
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/_AnonymousUserSidebarDisplay.cshtml")]
    public class _AnonymousUserSidebarDisplay : System.Web.Mvc.WebViewPage<MyJobLeads.Controllers.ActiveSidebarLink>
    {
        public _AnonymousUserSidebarDisplay()
        {
        }
        public override void Execute()
        {


WriteLiteral("\r\n<h2 id=\"accountName\">Not Logged In</h2>\r\n\r\n<div id=\"leftHandNav\">\r\n    <ul>\r\n  " +
"      <li>");


            
            #line 8 "..\..\Views\Home\_AnonymousUserSidebarDisplay.cshtml"
       Write(Html.ActionLink("Log In", MVC.Account.LogOn(), new { @class = Model == ActiveSidebarLink.Logon ? "active" : "" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n        <li>");


            
            #line 9 "..\..\Views\Home\_AnonymousUserSidebarDisplay.cshtml"
       Write(Html.ActionLink("About", MVC.Home.About(), new { @class = Model == ActiveSidebarLink.About ? "active" : "" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n    </ul>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591
