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

namespace MyJobLeads.Areas.ContactSearch.Views.Jigsaw
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/ContactSearch/Views/Jigsaw/Authenticate.cshtml")]
    public class Authenticate : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.ContactSearch.Models.JigsawAuthenticateViewModel>
    {
        public Authenticate()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
  
    ViewBag.Title = "Jigsaw Authentication";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"pageInfoBox\"> \r\n");


            
            #line 9 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
         using (Html.BeginForm()) 
        {                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n          " +
"      <h3 class=\"floatLeft\">Jigsaw Authentication</h3> \r\n            </div> \r\n");


            
            #line 14 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 marginAuto floatLeft"">
                <div class=""floatLeft"">
                    <p>
                        In order to search for contacts in the Jigsaw database, we need to authenticate with your Jigsaw account.
                    </p>    
                </div>
            </div>
");


            
            #line 22 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatleft\">\r\n           " +
"     <div class=\"floatLeft infoSpan\">\r\n                    ");


            
            #line 25 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
               Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n");


            
            #line 28 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\"><p class=\"greyHighlight\">Username:</p>\r\n                    <d" +
"iv class=\"infoSpan\">");


            
            #line 31 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
                                     Write(Html.TextBoxFor(x => x.Username, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                </div> \r\n            </div> \r\n");


            
            #line 34 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\"><p class=\"greyHighlight\">Password:</p>\r\n                    <d" +
"iv class=\"infoSpan\">");


            
            #line 37 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
                                     Write(Html.PasswordFor(x => x.Password, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                </div> \r\n            </div> \r\n");


            
            #line 40 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                <div " +
"class=\"submitBTN \"><input type=\"submit\" value=\"Autheticate\" /></div>            " +
"        \r\n            </div> \r\n");


            
            #line 44 "..\..\Areas\ContactSearch\Views\Jigsaw\Authenticate.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div class=\"clear\"></div> \r\n    </div> \r\n</div> ");


        }
    }
}
#pragma warning restore 1591
