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

namespace MyJobLeads.Views.Shared
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_LogOnPartial.cshtml")]
    public class _LogOnPartial : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _LogOnPartial()
        {
        }
        public override void Execute()
        {

            
            #line 1 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
 if(Request.IsAuthenticated) {

            
            #line default
            #line hidden
WriteLiteral("    ");

WriteLiteral("Welcome <b>");


            
            #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
                Write(Context.User.Identity.Name);

            
            #line default
            #line hidden
WriteLiteral("</b>!\r\n    [ ");


            
            #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
 Write(Html.ActionLink("Change Password", MVC.Account.ChangePassword()));

            
            #line default
            #line hidden
WriteLiteral(" ]\r\n    [ ");


            
            #line 4 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
 Write(Html.ActionLink("Log Off", MVC.Account.LogOff()));

            
            #line default
            #line hidden
WriteLiteral(" ]");

WriteLiteral("\r\n");


            
            #line 5 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
}
else {

            
            #line default
            #line hidden
WriteLiteral("    ");

WriteLiteral("[ ");


            
            #line 7 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
   Write(Html.ActionLink("Log On", "LogOn", "Account"));

            
            #line default
            #line hidden
WriteLiteral(" ]\r\n");


            
            #line 8 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Shared\_LogOnPartial.cshtml"
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
