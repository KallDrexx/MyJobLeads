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

namespace MyJobLeads.Views.Company
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Company/_CompanySummaryDisplay.cshtml")]
    public class _CompanySummaryDisplay : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.Companies.CompanySummaryViewModel>
    {
        public _CompanySummaryDisplay()
        {
        }
        public override void Execute()
        {

WriteLiteral("           \r\n<div class=\"grid1 marginBottom_20 floatLeft\"> \r\n    <div class=\"Bott" +
"omPageInfo\"> \r\n        <div class=\"grid3 marginAuto floatLeft\"> \r\n            <h" +
"3 class=\"floatLeft\">Company: ");


            
            #line 6 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
                                      Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</h3> \r\n            ");


            
            #line 7 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
       Write(Html.ActionLink("Edit Details", MVC.Company.Edit(Model.Id), new { @class = "floatRight blueLinks", title = "Edit Details" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div> \r\n\r\n        <div class=\"grid3 floatLeft\"> \r\n            <p class" +
"=\"floatLeft\"><span class=\"greyHighlight\">Phone: </span>");


            
            #line 11 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
                                                                      Write(Model.Phone);

            
            #line default
            #line hidden
WriteLiteral("</p> \r\n            <p class=\"floatRight\"><span class=\"greyHighlight\">Status: </sp" +
"an> ");


            
            #line 12 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
                                                                         Write(Model.LeadStatus);

            
            #line default
            #line hidden
WriteLiteral("</p> \r\n        </div> \r\n\r\n        <div class=\"grid3 marginBottom_10 floatLeft\"> \r" +
"\n            <p><span class=\"greyHighlight\">Location: </span>");


            
            #line 16 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
                                                       Write(Model.Location);

            
            #line default
            #line hidden
WriteLiteral("</p> \r\n        </div>\r\n         \r\n        <div class=\"grid3 marginBottom_10 float" +
"Left\">\r\n            <p>");


            
            #line 20 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
          Write(Html.Raw(Html.Encode(Model.Notes).Replace(Environment.NewLine, "<br />")));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            ");


            
            #line 21 "..\..\Views\Company\_CompanySummaryDisplay.cshtml"
       Write(Html.ActionLink("Show Details", MVC.Company.Details(Model.Id), new { @class = "blueLinks", title = "Show Details" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div> \r\n</div>\r\n\r\n");


        }
    }
}
#pragma warning restore 1591
