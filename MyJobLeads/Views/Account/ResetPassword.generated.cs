﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/ResetPassword.cshtml")]
    public class ResetPassword : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ResetPassword()
        {
        }
        public override void Execute()
        {

            
            #line 1 "..\..\Views\Account\ResetPassword.cshtml"
  
    ViewBag.Title = "Password Reset";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"pageInfoBox\"> \r\n");


            
            #line 7 "..\..\Views\Account\ResetPassword.cshtml"
         using (Html.BeginForm(MVC.Account.ResetPasswordResult())) 
        {
            

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n           " +
"     <h3>Reset Password</h3>\r\n            </div>\r\n");


            
            #line 13 "..\..\Views\Account\ResetPassword.cshtml"


            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 marginAuto floatLeft"">
                <p>
                    This page allows you to reset the password on your account.  After entering your email address, if your account
                    exists in the system we will generate a random password and email it to you.
                </p>
            </div>
");


            
            #line 20 "..\..\Views\Account\ResetPassword.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatleft\">\r\n           " +
"     <div class=\"floatLeft infoSpan\">\r\n                    ");


            
            #line 23 "..\..\Views\Account\ResetPassword.cshtml"
               Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n");


            
            #line 26 "..\..\Views\Account\ResetPassword.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\"><p class=\"greyHighlight\">Email:</p>\r\n                    <div " +
"class=\"infoSpan\">");


            
            #line 29 "..\..\Views\Account\ResetPassword.cshtml"
                                     Write(Html.TextBox("userEmail", (string)ViewBag.LastEmail, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                </div> \r\n            </div> \r\n");


            
            #line 32 "..\..\Views\Account\ResetPassword.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                <div " +
"class=\"submitBTN \"><input type=\"submit\" value=\"Reset Password\" /></div>         " +
"           \r\n            </div> \r\n");


            
            #line 36 "..\..\Views\Account\ResetPassword.cshtml"
            
        }

            
            #line default
            #line hidden
WriteLiteral("        <div class=\"clear\"></div>\r\n    </div>\r\n</div>");


        }
    }
}
#pragma warning restore 1591