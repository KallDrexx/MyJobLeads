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

namespace MyJobLeads.Areas.Organization.Views.Milestone
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Organization/Views/Milestone/Details.cshtml")]
    public class Details : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.ViewModels.Milestones.MilestoneDisplayViewModel>
    {
        public Details()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
  
    ViewBag.Title = "Milestone Details";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"pageInfoBox\">         \r\n       " +
" <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n            <h3 clas" +
"s=\"floatLeft\">Milestone Details</h3> \r\n            ");


            
            #line 11 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
       Write(Html.ActionLink("Edit Milestone", MVC.Organization.Milestone.Update(Model.Id), new { @class = "floatRight blueLinks", title = "Edit Milestone" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            ");


            
            #line 12 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
       Write(Html.ActionLink("Back To Milestone List", MVC.Organization.Milestone.List(), new { @class = "floatRight blueLinks", title = "Back To Milestone List" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n                \r\n        <div class=\"grid3 marginBottom_10 flo" +
"atLeft\"> \r\n            <div class=\"grid4 floatLeft\">\r\n                <p class=\"" +
"greyHighlight\">Title: <span class=\"setTask\">");


            
            #line 17 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                 Write(Model.Title);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                <p class=\"greyHighlight\">Number of Companies: <span " +
"class=\"setTask\">");


            
            #line 18 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                               Write(Model.JobSearchMetrics.NumCompaniesCreated);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                <p class=\"greyHighlight\">Number of Apply To Firm Tas" +
"ks Created: <span class=\"setTask\">");


            
            #line 19 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                                                 Write(Model.JobSearchMetrics.NumApplyTasksCreated);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                <p class=\"greyHighlight\">Number of Phone Interview T" +
"asks Created: <span class=\"setTask\">");


            
            #line 20 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                                                   Write(Model.JobSearchMetrics.NumPhoneInterviewTasksCreated);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n            </div> \r\n\r\n            <div class=\"grid4 floatLeft\">\r\n  " +
"              <p class=\"greyHighlight\">Previous Milestone: \r\n");


            
            #line 25 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                     if (Model.PreviousMilestoneId == 0)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span class=\"setTask\">First Milestone In Chain</span>\r\n");


            
            #line 28 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                    }
                    else
                    {
                        
            
            #line default
            #line hidden
            
            #line 31 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                   Write(Html.ActionLink(Model.PreviousMilestoneName, MVC.Organization.Milestone.Details(Model.PreviousMilestoneId), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
            
            #line 31 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                                                                                                                   
                    }

            
            #line default
            #line hidden
WriteLiteral("                </p>\r\n                <p class=\"greyHighlight\">Number of Contacts" +
": <span class=\"setTask\">");


            
            #line 34 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                              Write(Model.JobSearchMetrics.NumContactsCreated);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                <p class=\"greyHighlight\">Number of Apply To Firm Tas" +
"ks Completed: <span class=\"setTask\">");


            
            #line 35 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                                                   Write(Model.JobSearchMetrics.NumApplyTasksCompleted);

            
            #line default
            #line hidden
WriteLiteral("</span></p>\r\n                <p class=\"greyHighlight\">Number of In-Person Intervi" +
"ew Tasks Created: <span class=\"setTask\">");


            
            #line 36 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                                                                                       Write(Model.JobSearchMetrics.NumInPersonInterviewTasksCreated);

            
            #line default
            #line hidden
WriteLiteral(@"</span></p>
            </div> 
        </div>
        
        <div class=""grid3 marginBottom_10 floatLeft""> 
            <div class=""floatLeft"">
                <p class=""greyHighlight"">Instructional Text: 
                    <span class=""setTask"">");


            
            #line 43 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                     Write(Html.Raw(Html.Encode(Model.Instructions).Replace(Environment.NewLine, "<br />")));

            
            #line default
            #line hidden
WriteLiteral(@"</span>
                </p>
            </div> 
        </div> 

        <div class=""grid3 marginBottom_10 floatLeft""> 
            <div class=""floatLeft"">
                <p class=""greyHighlight"">Text Displayed Upon Milestone Completion: 
                    <span class=""setTask"">");


            
            #line 51 "..\..\Areas\Organization\Views\Milestone\Details.cshtml"
                                     Write(Html.Raw(Html.Encode(Model.CompletionDisplay).Replace(Environment.NewLine, "<br />")));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                </p>\r\n            </div> \r\n        </div> \r\n        \r\n  " +
"      <div class=\"clear\"></div> \r\n    </div> \r\n</div> \r\n\r\n");


        }
    }
}
#pragma warning restore 1591
