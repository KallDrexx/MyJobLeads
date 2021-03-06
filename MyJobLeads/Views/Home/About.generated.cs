﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyJobLeads.Views.Home
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/About.cshtml")]
    public class About : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.ViewModels.Organizations.VisibleOrgOfficialDocListForUserViewModel>
    {
        public About()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Views\Home\About.cshtml"
  
    ViewBag.Title = "About Us";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 8 "..\..\Views\Home\About.cshtml"
      Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.About));

            
            #line default
            #line hidden

});

WriteLiteral(@"

<div class=""grid1 floatLeft""> 
    <div class=""pageInfoBox""> 
        <div class=""grid3 marginBottom_10 marginAuto floatLeft"">
            <h3>About InterviewTools Portal</h3>
        </div>

        <div class=""grid3 marginBottom_10 marginAuto floatLeft"">
            <p>
                The InterviewTools Portal is your key to a successful job search by giving you the tools to efficiently organize your job leads.
            </p>
            <p>
                The InterviewTools Portal allows you to: 
                <ul class=""decoratedList"">
                    <li>Store companies and contacts - keep up to date notes about the progress of your job search in an organized fashion</li>
                    <li>Schedule follow-ups - to remind you when you need to contact a manager or human resources</li>
                    <li>Track your application - record which firms you've applied to and who you have contacted afterwards</li>
                </ul>
            </p>

");


            
            #line 30 "..\..\Views\Home\About.cshtml"
             if (Model.UserVisibleDocuments.Count > 0)
            {

            
            #line default
            #line hidden
WriteLiteral("                <p>\r\n                    The following document(s) are available " +
"to help guide you through using MyLeads:\r\n                    <ul class=\"decorat" +
"edList\">\r\n");


            
            #line 35 "..\..\Views\Home\About.cshtml"
                         foreach (var doc in Model.UserVisibleDocuments.OrderBy(x => x.Name))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li><a href=\"");


            
            #line 37 "..\..\Views\Home\About.cshtml"
                                    Write(doc.DownloadUrl);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"inlineBlue\">");


            
            #line 37 "..\..\Views\Home\About.cshtml"
                                                                         Write(doc.Name);

            
            #line default
            #line hidden
WriteLiteral("</a></li>\r\n");


            
            #line 38 "..\..\Views\Home\About.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </ul>\r\n                </p>\r\n");


            
            #line 41 "..\..\Views\Home\About.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral(@"
            <p>
                For questions or information, please contact us at <a class=""inlineBlue"" href=""mailto:support@interviewtools.net"">support@interviewtools.net</a>
            </p>
        </div>
            
        <div class=""clear""></div>
    </div>
</div>");


        }
    }
}
#pragma warning restore 1591
