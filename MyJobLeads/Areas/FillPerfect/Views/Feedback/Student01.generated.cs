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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/FillPerfect/Views/Feedback/Student01.cshtml")]
    public class Student01 : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.FillPerfect.Models.Surveys.StudentSurveyPost3ViewModel>
    {
        public Student01()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
  
    ViewBag.Title = "Student Survey";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
 using (Html.PageInfoBox(false))
{
    using (Html.BeginForm())
    {
        
            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
   Write(Html.HiddenFor(x => x.FpUserId));

            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                                        
        
        using (Html.OuterRow())
        {

            
            #line default
            #line hidden
WriteLiteral("            <h3>Student Survey</h3>\r\n");


            
            #line 16 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
        }
       
        using (Html.OuterRow())
        {
            using (Html.ContentArea())
            {

            
            #line default
            #line hidden
WriteLiteral("                <p>\r\n                    ");


            
            #line 23 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
               Write(Model.Question);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </p>\r\n");


            
            #line 25 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
            }
        }

        using (Html.OuterRow())
        {
            using (Html.ValidationArea())
            {
                
            
            #line default
            #line hidden
            
            #line 32 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
           Write(Html.ValidationSummary());

            
            #line default
            #line hidden
            
            #line 32 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                                         
            }
        }

        using (Html.OuterRow())
        {
            using (Html.FormFieldArea(new HtmlString("")))
            {

            
            #line default
            #line hidden
WriteLiteral(@"                <table>
                    <tr>
                        <td align=""left"">&nbsp; 1</td>
                        <td align=""right"">5&nbsp;</td>
                    </tr>

                    <tr>
                        <td colspan=""3"">
                            ");


            
            #line 48 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                       Write(Html.RadioButtonFor(x => x.Answer, 1));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            ");


            
            #line 49 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                       Write(Html.RadioButtonFor(x => x.Answer, 2));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            ");


            
            #line 50 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                       Write(Html.RadioButtonFor(x => x.Answer, 3));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            ");


            
            #line 51 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                       Write(Html.RadioButtonFor(x => x.Answer, 4));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            ");


            
            #line 52 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
                       Write(Html.RadioButtonFor(x => x.Answer, 5));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                </tab" +
"le>\r\n");


            
            #line 56 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("                ");

WriteLiteral("\r\n                    &nbsp; 1 means that you do <span class=\"bold\">not</span> th" +
"ink this tool will be useful <br />\r\n                    &nbsp; 5 means that you" +
" think this tool will be useful <br />\r\n                ");

WriteLiteral("\r\n");


            
            #line 61 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
            }
        }

        using (Html.OuterRow())
        {
            Html.FormField("Additional Comments", Html.TextAreaFor(x => x.Comments, new { @class = "textAreaInfo" }));
        }

        using (Html.OuterRow())
        {
            using (Html.FormButtonArea())
            {

            
            #line default
            #line hidden
WriteLiteral("                <input type=\"submit\" value=\"Submit\" />\r\n");


            
            #line 74 "..\..\Areas\FillPerfect\Views\Feedback\Student01.cshtml"
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