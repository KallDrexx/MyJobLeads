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

namespace MyJobLeads.Areas.FillPerfect.Views.Feedback
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/FillPerfect/Views/Feedback/SurveyCompleted.cshtml")]
    public class SurveyCompleted : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.FillPerfect.Models.Surveys.SurveyCompletedViewModel>
    {
        public SurveyCompleted()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\FillPerfect\Views\Feedback\SurveyCompleted.cshtml"
  
    ViewBag.Title = "Survey Completed";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "..\..\Areas\FillPerfect\Views\Feedback\SurveyCompleted.cshtml"
 using (Html.PageInfoBox(false))
{
    using (Html.OuterRow())
    {

            
            #line default
            #line hidden
WriteLiteral("        <h3>Survey Completed</h3>\r\n");


            
            #line 12 "..\..\Areas\FillPerfect\Views\Feedback\SurveyCompleted.cshtml"
    }

    using (Html.OuterRow())
    {
        using (Html.ContentArea())
        {

            
            #line default
            #line hidden
WriteLiteral("            <p>\r\n");


            
            #line 19 "..\..\Areas\FillPerfect\Views\Feedback\SurveyCompleted.cshtml"
                 if (Model.SurveyPreviouslyCompleted)
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

WriteLiteral("You have already completed this survey.\r\n");


            
            #line 22 "..\..\Areas\FillPerfect\Views\Feedback\SurveyCompleted.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n                Thank you for your feedback!\r\n            </p>\r\n");


            
            #line 26 "..\..\Areas\FillPerfect\Views\Feedback\SurveyCompleted.cshtml"
        }
    }
}
            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591