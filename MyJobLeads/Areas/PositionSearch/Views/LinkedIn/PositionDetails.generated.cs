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

namespace MyJobLeads.Areas.PositionSearch.Views.LinkedIn
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/PositionSearch/Views/LinkedIn/PositionDetails.cshtml")]
    public class PositionDetails : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.ViewModels.PositionSearching.ExternalPositionDetailsViewModel>
    {
        public PositionDetails()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
  
    ViewBag.Title = "LinkedIn Position Details";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"pageInfoBox\">\r\n");


            
            #line 9 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
         if (Model == null)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n          " +
"      <h3 class=\"floatLeft\">Position Not Found</h3> \r\n            </div> \r\n");


            
            #line 14 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n           " +
"     <p>LinkedIn could not find the position specified</p>\r\n            </div>\r\n" +
"");


            
            #line 18 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
        }

        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n          " +
"      <h3 class=\"floatLeft\">");


            
            #line 23 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                 Write(Model.Title);

            
            #line default
            #line hidden
WriteLiteral("</h3> \r\n                ");


            
            #line 24 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
           Write(Html.ActionLink("Add To Your Job Search", 
                        MVC.PositionSearch.LinkedIn.AddPosition(Model.Id, Model.Title, Model.CompanyName),
                        new { @class = "floatRight blueLinks", title = "Add Position To Your Job Search" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div> \r\n");


            
            #line 28 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"grid4 floatLeft\"> \r\n                    <p class=\"greyHighlight\">Company:" +
" <span class=\"setTask\">");


            
            #line 31 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                       Write(Model.CompanyName);

            
            #line default
            #line hidden
WriteLiteral("</span></p> \r\n                    <p class=\"greyHighlight\">Location: <span class=" +
"\"setTask\">");


            
            #line 32 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                        Write(Model.Location);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                    <p class=\"greyHighlight\">Posted On: <span class=" +
"\"setTask\">");


            
            #line 33 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                         Write(Model.PostedDate.ToLongDateString());

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                    <p class=\"greyHighlight\">Active Posting?: <span " +
"class=\"setTask\">");


            
            #line 34 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                                Write(Model.IsActive ? "Yes" : "No");

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                    <p class=\"greyHighlight\">Job Type: <span class=\"" +
"setTask\">");


            
            #line 35 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                        Write(Model.JobType);

            
            #line default
            #line hidden
WriteLiteral("</span></p> \r\n                    <p class=\"greyHighlight\">Experience: <span clas" +
"s=\"setTask\">");


            
            #line 36 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                          Write(Model.ExperienceLevel);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                    <p class=\"greyHighlight\">Job Functions: <span cl" +
"ass=\"setTask\">");


            
            #line 37 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                             Write(Model.JobFunctions);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                    <p class=\"greyHighlight\">Industries: <span class" +
"=\"setTask\">");


            
            #line 38 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                          Write(Model.Industries);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                </div> \r\n\r\n                <div class=\"grid4 floatRi" +
"ght\">  \r\n                    <p class=\"greyHighlight\">Job Poster: <span class=\"s" +
"etTask\">");


            
            #line 42 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                          Write(Model.JobPosterName);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                    <p class=\"greyHighlight\">Headline: <span class=\"" +
"setTask\">");


            
            #line 43 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                                                        Write(Model.JobPosterHeadline);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                </div> \r\n            </div> \r\n");


            
            #line 46 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\">\r\n                    <p class=\"greyHighlight\">Description: \r\n" +
"                        <span class=\"setTask\">");


            
            #line 50 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
                                         Write(Html.Raw(Model.Description));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                    </p>\r\n                </div> \r\n            </div> \r\n" +
"");


            
            #line 54 "..\..\Areas\PositionSearch\Views\LinkedIn\PositionDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div class=\"clear\"></div>\r\n    </div> \r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591
