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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/ChangePassword.cshtml")]
    public class ChangePassword : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.ChangePasswordModel>
    {
        public ChangePassword()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
  
    ViewBag.Title = "Change Password";


            
            #line default
            #line hidden
WriteLiteral("\r\n<h2>Change Password</h2>\r\n<p>\r\n    Use the form below to change your password. " +
"\r\n</p>\r\n<p>\r\n    New passwords are required to be a minimum of ");


            
            #line 12 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
                                             Write(ViewBag.PasswordLength);

            
            #line default
            #line hidden
WriteLiteral(" characters in length.\r\n</p>\r\n\r\n<script src=\"");


            
            #line 15 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
        Write(Url.Content("~/Scripts/jquery.validate.min.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n<script src=\"");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
        Write(Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n\r\n");


            
            #line 18 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
 using (Html.BeginForm()) {
    
            
            #line default
            #line hidden
            
            #line 19 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
Write(Html.ValidationSummary(true, "Password change was unsuccessful. Please correct the errors and try again."));

            
            #line default
            #line hidden
            
            #line 19 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
                                                                                                               

            
            #line default
            #line hidden
WriteLiteral("    <div>\r\n        <fieldset>\r\n            <legend>Account Information</legend>\r\n" +
"\r\n            <div class=\"editor-label\">\r\n                ");


            
            #line 25 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.LabelFor(m => m.OldPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div class=\"editor-field\">\r\n                ");


            
            #line 28 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.PasswordFor(m => m.OldPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");


            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.ValidationMessageFor(m => m.OldPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"editor-label\">\r\n                ");


            
            #line 33 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.LabelFor(m => m.NewPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div class=\"editor-field\">\r\n                ");


            
            #line 36 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.PasswordFor(m => m.NewPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");


            
            #line 37 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.ValidationMessageFor(m => m.NewPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"editor-label\">\r\n                ");


            
            #line 41 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.LabelFor(m => m.ConfirmPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div class=\"editor-field\">\r\n                ");


            
            #line 44 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.PasswordFor(m => m.ConfirmPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");


            
            #line 45 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
           Write(Html.ValidationMessageFor(m => m.ConfirmPassword));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <p>\r\n                <input type=\"submit\" val" +
"ue=\"Change Password\" />\r\n            </p>\r\n        </fieldset>\r\n    </div>\r\n");


            
            #line 53 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ChangePassword.cshtml"
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
