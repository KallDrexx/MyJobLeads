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

namespace MyJobLeads.Views.Account
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
    using MyJobLeads.Infrastructure.HtmlHelpers;
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.1.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/LogOn.cshtml")]
    public class LogOn : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.LogOnModel>
    {
        public LogOn()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
  
    ViewBag.Title = "Log On";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 8 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
       Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.Logon)); 

            
            #line default
            #line hidden

});

WriteLiteral("\r\n\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"lineSeperater\"> \r\n        <di" +
"v class=\"pageInfoBox\"> \r\n");


            
            #line 14 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
             using (Html.BeginForm()) 
            {

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n      " +
"              <h3 class=\"floatLeft\">Log On</h3> \r\n                </div> \r\n");


            
            #line 19 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n       " +
"             <p>\r\n                        If you do not have an account, you can" +
" ");


            
            #line 22 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                                                          Write(Html.ActionLink("create one now", MVC.Account.Register(), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
WriteLiteral(" <br />\r\n                        If you have forgotten your password, you can ");


            
            #line 23 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                                                                Write(Html.ActionLink("generate a new one", MVC.Account.ResetPassword(), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </p>    \r\n                </div>\r\n");


            
            #line 26 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatleft\">\r\n       " +
"             <div class=\"floatLeft infoSpan\">\r\n                        ");


            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                   Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </div>\r\n");


            
            #line 32 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Email:</p>\r\n                 " +
"       <div class=\"infoSpan\">");


            
            #line 35 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                                         Write(Html.TextBoxFor(x => x.UserName, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 38 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Password:</p>\r\n              " +
"          <div class=\"infoSpan\">");


            
            #line 41 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                                         Write(Html.PasswordFor(x => x.Password, new { @class = "info "}));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 44 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                 " +
"   <div class=\"submitBTN \"><input type=\"submit\" value=\"Log On\" /></div>         " +
"           \r\n                </div> \r\n");


            
            #line 48 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div class=\"clear\"></div> \r\n        </div> \r\n    </div> \r\n</div> ");


        }
    }
}
#pragma warning restore 1591
