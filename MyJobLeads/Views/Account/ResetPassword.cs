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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/ResetPassword.cshtml")]
    public class ResetPassword : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ResetPassword()
        {
        }
        public override void Execute()
        {

            
            #line 1 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ResetPassword.cshtml"
  
    ViewBag.Title = "Password Reset";
    Layout = "~/Views/Shared/_Layout.cshtml";


            
            #line default
            #line hidden
WriteLiteral("\r\n<h2>Reset User Password</h2>\r\n\r\nEnter your email to reset your password:<br />\r" +
"\n");


            
            #line 9 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ResetPassword.cshtml"
 using (Html.BeginForm(MVC.Account.ResetPasswordResult()))
{
    
            
            #line default
            #line hidden
            
            #line 11 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ResetPassword.cshtml"
Write(Html.TextBox("userEmail"));

            
            #line default
            #line hidden

WriteLiteral(" <input type=\"submit\" value=\"Reset\" />\r\n");


            
            #line 12 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Account\ResetPassword.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");


        }
    }
}
#pragma warning restore 1591