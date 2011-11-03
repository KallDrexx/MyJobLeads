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

namespace MyJobLeads.Areas.Organization.Views.Dashboard
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
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Organization/Views/Dashboard/Index.cshtml")]
    public class Index : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.ViewModels.Organizations.OrganizationDashboardViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
  
    ViewBag.Title = "University Dashboard";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 8 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
      Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.OrgAdmin));

            
            #line default
            #line hidden

});

WriteLiteral("\r\n\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"pageInfoBox\">         \r\n     " +
"   <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n            <h3 cl" +
"ass=\"floatLeft\">University Dashboard</h3> \r\n            ");


            
            #line 15 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
       Write(Html.ActionLink("Edit University Options", MVC.Organization.EditOrganization.Index(Model.Organization.Id), new { @class = "floatRight blueLinks", title = "Edit University Options" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            ");


            
            #line 16 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
       Write(Html.ActionLink("View Milestones", MVC.Organization.Milestone.List(), new { @class = "floatRight blueLinks", title = "View Milestones" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            ");


            
            #line 17 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
       Write(Html.ActionLink("View Student Statistics", MVC.Organization.MemberStats.Index(Model.Organization.Id),
                                                 new { @class = "floatRight blueLinks", title = "Detailed Student Statistics" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div> \r\n                \r\n        <div class=\"grid3 marginBottom_10 fl" +
"oatLeft\"> \r\n            <div class=\"floatLeft\"><p class=\"greyHighlight\">Universi" +
"ty Name:</p>\r\n                <div class=\"infoSpan\">");


            
            #line 23 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                 Write(Model.Organization.Name);

            
            #line default
            #line hidden
WriteLiteral(@"</div>
            </div> 
        </div> 

        <div class=""grid3 marginBottom_10 floatLeft""> 
            <div class=""floatLeft""><p class=""greyHighlight"">Student Usage Summary:</p>
                <div class=""infoSpan"">
                    Number of registered students: <span class=""bold"">");


            
            #line 30 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                 Write(Model.NumMembers);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of entered companies: <span class=\"bol" +
"d\">");


            
            #line 31 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                               Write(Model.NumCompanies);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of entered contacts: <span class=\"bold" +
"\">");


            
            #line 32 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                              Write(Model.NumContacts);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of positions students have applied to:" +
" <span class=\"bold\">");


            
            #line 33 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                                Write(Model.NumAppliedPositions);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of positions students have not applied" +
" to: <span class=\"bold\">");


            
            #line 34 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                                    Write(Model.NumNotAppliedPositions);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of scheduled phone interviews: <span c" +
"lass=\"bold\">");


            
            #line 35 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                        Write(Model.NumOpenPhoneTasks);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of completed phone interviews: <span c" +
"lass=\"bold\">");


            
            #line 36 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                        Write(Model.NumClosedPhoneTasks);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of scheduled in-person interviews: <sp" +
"an class=\"bold\">");


            
            #line 37 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                            Write(Model.NumOpenInterviewTasks);

            
            #line default
            #line hidden
WriteLiteral("</span> <br />\r\n                    Number of completed in-person interviews: <sp" +
"an class=\"bold\">");


            
            #line 38 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                            Write(Model.NumClosedInterviewTasks);

            
            #line default
            #line hidden
WriteLiteral(@"</span> <br />
                </div>
            </div> 
        </div> 

        <div class=""grid3 marginBottom_10 floatLeft""> 
            <div class=""floatLeft""><p class=""greyHighlight"">Registration URL for Students:</p>
                <div class=""infoSpan"">
                    ");


            
            #line 46 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
               Write(Html.ActionLink(Url.Action("RegisterWithOrganization", "Account", new { area = "", registrationToke = Model.Organization.RegistrationToken }),
                        MVC.Account.RegisterWithOrganization(Model.Organization.RegistrationToken), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div> \r\n        </div> \r\n\r\n");


            
            #line 52 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
         if (Model.Organization.IsEmailDomainRestricted)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\">\r\n                    <p class=\"greyHighlight\">Students must r" +
"egister with an email from the <span class=\"greyHighlight\">");


WriteLiteral("@");


            
            #line 56 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                                                                    Write(Model.Organization.EmailDomains.First().Domain);

            
            #line default
            #line hidden
WriteLiteral("</span> domain</p>\r\n                </div> \r\n            </div>\r\n");


            
            #line 59 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
        }

        else
        {

            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 floatLeft""> 
                <div class=""floatLeft""><p class=""greyHighlight""></p>
                    <div class=""infoSpan"">Students can register with any email address</div>
                </div> 
            </div>
");


            
            #line 68 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 70 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
         if (Model.NonMemberOfficialDocuments.Count > 0)
        {

            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 floatLeft"">
                <div class=""floatLeft"">
                    <p class=""greyHighlight"">Documentation For Career Services (not visible to students):</p>
                    <div class=""infoSpan"">
                        <ul>
");


            
            #line 77 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                             foreach (var doc in Model.NonMemberOfficialDocuments)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li><a href=\"");


            
            #line 79 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                        Write(doc.DownloadUrl);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"inlineBlue\">");


            
            #line 79 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                             Write(doc.Name);

            
            #line default
            #line hidden
WriteLiteral("</a></li>\r\n");


            
            #line 80 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </ul>\r\n                    </div>\r\n                </div>" +
"\r\n            </div>\r\n");


            
            #line 85 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        \r\n        <div class=\"grid3 marginBottom_10 floatLeft\">\r\n            <div" +
" class=\"floatLeft\">\r\n                <p class=\"greyHighlight\">Documentation Visi" +
"ble To Students:</p>\r\n                <div class=\"infoSpan\">\r\n                  " +
"  <ul>\r\n\r\n");


            
            #line 93 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                         if (Model.Organization.MemberOfficialDocuments.Count > 0)
                        {
                            foreach (var doc in Model.Organization.MemberOfficialDocuments)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li>\r\n                                    <a href" +
"=\"");


            
            #line 98 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                        Write(doc.DownloadUrl);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"inlineBlue\">");


            
            #line 98 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                             Write(doc.Name);

            
            #line default
            #line hidden
WriteLiteral("</a> - \r\n                                    [ ");


            
            #line 99 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                 Write(Html.ActionLink("Hide From Students", MVC.Organization.Dashboard.HideDocumentFromMembers(doc.Id), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
WriteLiteral(" ]\r\n                                </li>\r\n");


            
            #line 101 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                            }
                        }

                        else
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li>No documentation has been selected to be visible " +
"students.</li>\r\n");


            
            #line 107 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral(@"
                    </ul>
                </div>
            </div>
        </div>

        <div class=""grid3 marginBottom_10 floatLeft"">
            <div class=""floatLeft"">
                <p class=""greyHighlight"">Documentation NOT Visible To Students:</p>
                <div class=""infoSpan"">
                    <ul>

");


            
            #line 120 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                         if (Model.HiddenMemberDocuments.Count > 0)
                        {
                            foreach (var doc in Model.HiddenMemberDocuments)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li>\r\n                                    <a href" +
"=\"");


            
            #line 125 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                        Write(doc.DownloadUrl);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"inlineBlue\">");


            
            #line 125 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                                                             Write(doc.Name);

            
            #line default
            #line hidden
WriteLiteral("</a> - \r\n                                    [ ");


            
            #line 126 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                                 Write(Html.ActionLink("Show To Students", MVC.Organization.Dashboard.ShowDocumentToMembers(doc.Id), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
WriteLiteral(" ]\r\n                                </li>\r\n");


            
            #line 128 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                            }
                        }

                        else
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li>All documentation is visible to students</li>\r\n");


            
            #line 134 "..\..\Areas\Organization\Views\Dashboard\Index.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </ul>\r\n                </div>\r\n            </div>\r\n        " +
"</div>\r\n\r\n        <div class=\"clear\"></div> \r\n    </div> \r\n</div> ");


        }
    }
}
#pragma warning restore 1591
