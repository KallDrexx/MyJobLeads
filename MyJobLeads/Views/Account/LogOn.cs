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
    using System.Web.Mvc.Html;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
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
WriteLiteral("\r\n<h2>Log On</h2>\r\n<p>\r\n    Please enter your username and password. ");


            
            #line 9 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
                                        Write(Html.ActionLink("Register", "Register"));

            
            #line default
            #line hidden
WriteLiteral(" if you don\'t have an account\r\n    or ");


            
            #line 10 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
  Write(Html.ActionLink("Reset", MVC.Account.ResetPassword()));

            
            #line default
            #line hidden
WriteLiteral(" your password if you forgot it.\r\n</p>\r\n\r\n<script src=\"");


            
            #line 13 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
        Write(Url.Content("~/Scripts/jquery.validate.min.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n<script src=\"");


            
            #line 14 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
        Write(Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n\r\n");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
Write(Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again."));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");


            
            #line 18 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
 using (Html.BeginForm()) {

            
            #line default
            #line hidden
WriteLiteral("    <div>\r\n        <fieldset>\r\n            <legend>Account Information</legend>\r\n" +
"\r\n            <div class=\"editor-label\">\r\n                ");


            
            #line 24 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.LabelFor(m => m.UserName));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div class=\"editor-field\">\r\n                ");


            
            #line 27 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.TextBoxFor(m => m.UserName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");


            
            #line 28 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.ValidationMessageFor(m => m.UserName));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"editor-label\">\r\n                ");


            
            #line 32 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.LabelFor(m => m.Password));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div class=\"editor-field\">\r\n                ");


            
            #line 35 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.PasswordFor(m => m.Password));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");


            
            #line 36 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.ValidationMessageFor(m => m.Password));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"editor-label\">\r\n                ");


            
            #line 40 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.CheckBoxFor(m => m.RememberMe));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");


            
            #line 41 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
           Write(Html.LabelFor(m => m.RememberMe));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <p>\r\n                <input type=\"submit\" val" +
"ue=\"Log On\" />\r\n            </p>\r\n        </fieldset>\r\n    </div>\r\n");


            
            #line 49 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\LogOn.cshtml"
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
