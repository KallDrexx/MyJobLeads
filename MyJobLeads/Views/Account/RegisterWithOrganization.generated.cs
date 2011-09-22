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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/RegisterWithOrganization.cshtml")]
    public class RegisterWithOrganization : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.Accounts.OrganizationRegistrationViewModel>
    {
        public RegisterWithOrganization()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Views\Account\RegisterWithOrganization.cshtml"
  
    ViewBag.Title = "RegisterWithOrganization";
    Layout = "~/Views/Shared/_Layout.cshtml";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 9 "..\..\Views\Account\RegisterWithOrganization.cshtml"
      Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.Register));

            
            #line default
            #line hidden

});

WriteLiteral("\r\n\r\n<script src=\"");


            
            #line 12 "..\..\Views\Account\RegisterWithOrganization.cshtml"
        Write(Url.Content("~/Scripts/jquery.validate.min.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n<script src=\"");


            
            #line 13 "..\..\Views\Account\RegisterWithOrganization.cshtml"
        Write(Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n\r\n<div class=\"grid1 floatLeft\"> \r\n    <div cl" +
"ass=\"lineSeperater\"> \r\n        <div class=\"pageInfoBox\"> \r\n");


            
            #line 18 "..\..\Views\Account\RegisterWithOrganization.cshtml"
             using (Html.BeginForm(MVC.Account.RegisterWithOrganization(Model.RegistrationToken)))
            {
                
            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\Account\RegisterWithOrganization.cshtml"
           Write(Html.HiddenFor(x => x.RegistrationToken));

            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                                         
                
            
            #line default
            #line hidden
            
            #line 21 "..\..\Views\Account\RegisterWithOrganization.cshtml"
           Write(Html.HiddenFor(x => x.OrganizationName));

            
            #line default
            #line hidden
            
            #line 21 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                                        
                
            
            #line default
            #line hidden
            
            #line 22 "..\..\Views\Account\RegisterWithOrganization.cshtml"
           Write(Html.HiddenFor(x => x.MinPasswordLength));

            
            #line default
            #line hidden
            
            #line 22 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                                         
                
            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Account\RegisterWithOrganization.cshtml"
           Write(Html.HiddenFor(x => x.RestrictedEmailDomain));

            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                                             
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n      " +
"              <h3 class=\"floatLeft\">Register With The ");


            
            #line 26 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                                       Write(Model.OrganizationName);

            
            #line default
            #line hidden
WriteLiteral("</h3> \r\n                </div> \r\n");


            
            #line 28 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n       " +
"             <div class=\"floatLeft\">\r\n                        <p>\r\n             " +
"               As a member of the ");


            
            #line 32 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                          Write(Model.OrganizationName);

            
            #line default
            #line hidden
WriteLiteral(" you are entitled to a free MyLeads account.\r\n                        </p>    \r\n " +
"                   </div>\r\n                </div>\r\n");


            
            #line 36 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                
                if (!string.IsNullOrWhiteSpace(Model.RestrictedEmailDomain))
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n   " +
"                     <div class=\"floatLeft\">\r\n                            <p>\r\n " +
"                               ");


            
            #line 42 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                           Write(Model.OrganizationName);

            
            #line default
            #line hidden
WriteLiteral(" only allows students to sign up with an email address ending with: \r\n           " +
"                         <span class=\"greyHighlight\">");


WriteLiteral("@");


            
            #line 43 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                                             Write(Model.RestrictedEmailDomain);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                            </p>\r\n                        </div>\r\n      " +
"              </div>\r\n");


            
            #line 47 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                }
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatleft\">\r\n       " +
"             <div class=\"floatLeft infoSpan\">\r\n                        ");


            
            #line 51 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                   Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </div>\r\n");


            
            #line 54 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Full Name:</p>\r\n             " +
"           <div class=\"infoSpan\">");


            
            #line 57 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                         Write(Html.TextBoxFor(x => x.FullName, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 60 "..\..\Views\Account\RegisterWithOrganization.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Email Address:</p>\r\n         " +
"               <div class=\"infoSpan\">");


            
            #line 63 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                         Write(Html.TextBoxFor(x => x.Email, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div>\r\n");


            
            #line 66 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Password:</p>\r\n              " +
"          <div class=\"infoSpan\">");


            
            #line 69 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                         Write(Html.PasswordFor(x => x.Password, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 72 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Confirm Password:</p>\r\n      " +
"                  <div class=\"infoSpan\">");


            
            #line 75 "..\..\Views\Account\RegisterWithOrganization.cshtml"
                                         Write(Html.PasswordFor(x => x.ConfirmPassword, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div>\r\n");


            
            #line 78 "..\..\Views\Account\RegisterWithOrganization.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                 " +
"   <div class=\"submitBTN \"><input type=\"submit\" value=\"Register\" /></div>       " +
"             \r\n                </div> \r\n");


            
            #line 82 "..\..\Views\Account\RegisterWithOrganization.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div class=\"clear\"></div> \r\n        </div> \r\n    </div> \r\n</div>");


        }
    }
}
#pragma warning restore 1591
