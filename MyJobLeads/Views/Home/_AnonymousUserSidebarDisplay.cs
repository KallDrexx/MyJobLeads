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
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.1.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/_AnonymousUserSidebarDisplay.cshtml")]
    public class _AnonymousUserSidebarDisplay : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _AnonymousUserSidebarDisplay()
        {
        }
        public override void Execute()
        {
WriteLiteral("<p id=\"sidebar-links\">\r\n    ");


            
            #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_AnonymousUserSidebarDisplay.cshtml"
Write(Html.ActionLink("About", MVC.Home.About()));

            
            #line default
            #line hidden
WriteLiteral(" <br /><br />\r\n\r\n    ");


            
            #line 4 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_AnonymousUserSidebarDisplay.cshtml"
Write(Html.ActionLink("Log In", MVC.Account.LogOn()));

            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n    ");


            
            #line 5 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Home\_AnonymousUserSidebarDisplay.cshtml"
Write(Html.ActionLink("Register Account", MVC.Account.Register()));

            
            #line default
            #line hidden
WriteLiteral("\r\n</p>");


        }
    }
}
#pragma warning restore 1591
