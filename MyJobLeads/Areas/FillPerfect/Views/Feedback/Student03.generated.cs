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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/FillPerfect/Views/Feedback/Student03.cshtml")]
    public class Student03 : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.FillPerfect.Models.Surveys.StudentSurveyPost10ViewModel>
    {
        public Student03()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
  
    ViewBag.Title = "Student Survey";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
 using (Html.PageInfoBox(false))
{
    using (Html.BeginForm())
    {
        
            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
   Write(Html.HiddenFor(x => x.FpUserId));

            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
                                        
        
        using (Html.OuterRow())
        {

            
            #line default
            #line hidden
WriteLiteral("            <h3>Student Survey</h3>\r\n");


            
            #line 16 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
        }
       
        using (Html.OuterRow())
        {
            using (Html.ContentArea())
            {

            
            #line default
            #line hidden
WriteLiteral("                <p>\r\n                    ");


            
            #line 23 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
               Write(Model.Question);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </p>\r\n");


            
            #line 25 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
            }
        }

        using (Html.OuterRow())
        {
            using (Html.ValidationArea())
            {
                
            
            #line default
            #line hidden
            
            #line 32 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
           Write(Html.ValidationSummary());

            
            #line default
            #line hidden
            
            #line 32 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
                                         
            }
        }

        using (Html.OuterRow())
        {
            string label = "Some topics you may want to discuss are:<br><ul class=\"decoratedList\">" +
                "<li>Has it made you more productive?</li><li>Has FillPerfect helped you become more accurate?</li>" +
                "<li>Does FillPerfect recognize and complete enough fields for you on applications?</li>" +
                "<li>Would other students benefit from this tool?</li></ul>";
                
            Html.FormField(label, Html.TextAreaFor(x => x.Comments, new { @class = "textAreaInfo" }));
        }

        using (Html.OuterRow())
        {
            using (Html.FormButtonArea())
            {

            
            #line default
            #line hidden
WriteLiteral("                <input type=\"submit\" value=\"Submit\" />\r\n");


            
            #line 51 "..\..\Areas\FillPerfect\Views\Feedback\Student03.cshtml"
            }
        }
    }
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
