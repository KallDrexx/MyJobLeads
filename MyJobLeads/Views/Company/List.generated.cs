﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Company/List.cshtml")]
    public class List : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.Companies.JobSearchCompanyListViewModel>
    {
        public List()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Views\Company\List.cshtml"
  
    ViewBag.Title = "Company List";
    string companyWord = Model.TotalCompanyCount == 1 ? "company" : "companies";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 9 "..\..\Views\Company\List.cshtml"
      Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.Companies));

            
            #line default
            #line hidden

});

WriteLiteral("\r\n\r\n<div class=\"grid1 floatLeft\">\r\n    <div class=\"lineSeperator\">\r\n        <div " +
"class=\"pageInfoBox\">\r\n");


            
            #line 15 "..\..\Views\Company\List.cshtml"
             using (Html.BeginForm(MVC.Company.ChangeCompanyStatusFilters()))
            {

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 floatLeft\">\r\n                    <h3 class=\"flo" +
"atLeft\">List All Companies</h3>\r\n                    <p class=\"floatRight greyHi" +
"ghlight\">You have ");


            
            #line 19 "..\..\Views\Company\List.cshtml"
                                                             Write(Model.TotalCompanyCount == 0? "no" : Model.TotalCompanyCount.ToString());

            
            #line default
            #line hidden
WriteLiteral(" companies entered</p>\r\n                </div>\r\n");


            
            #line 21 "..\..\Views\Company\List.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\">\r\n                  " +
"  ");


            
            #line 23 "..\..\Views\Company\List.cshtml"
               Write(Html.ActionLink("Add New Company", MVC.Company.Add(), new { @class = "blueLinks", title = "Add New Company" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");


            
            #line 25 "..\..\Views\Company\List.cshtml"

                if (Model.UsedStatuses.Count > 0)
                {
                    
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\Company\List.cshtml"
               Write(Html.Hidden("jobSearchId", Model.JobSearchId));

            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\Company\List.cshtml"
                                                                  
                    

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"grid3 marginBottom_10 floatLeft\">\r\n              " +
"          <p>Show all firms with the following status:</p>\r\n                    " +
"</div>\r\n");


            
            #line 33 "..\..\Views\Company\List.cshtml"

                    foreach (string status in Model.UsedStatuses.OrderBy(x => x))
                    {
                        string checkedString = !Model.HiddenStatuses.Contains(status) ? "checked" : "";
                            

            
            #line default
            #line hidden
WriteLiteral("                        <div class=\"grid3 marginBottom_10 floatLeft\">\r\n          " +
"                  <div class=\"floatLeft infoSpan\">\r\n                            " +
"    <input type=\"checkbox\" name=\"shownStatus\" value=\"");


            
            #line 40 "..\..\Views\Company\List.cshtml"
                                                                            Write(status);

            
            #line default
            #line hidden
WriteLiteral("\" ");


            
            #line 40 "..\..\Views\Company\List.cshtml"
                                                                                     Write(checkedString);

            
            #line default
            #line hidden
WriteLiteral(" />");


            
            #line 40 "..\..\Views\Company\List.cshtml"
                                                                                                      Write(status);

            
            #line default
            #line hidden
WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n");


            
            #line 43 "..\..\Views\Company\List.cshtml"
                    }
                

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"grid3 marginBottom_20 floatLeft\">\r\n              " +
"          <div class=\"submitBTN\"><input type=\"submit\" value=\"Filter Companies\" /" +
"></div>\r\n                    </div>\r\n");


            
            #line 48 "..\..\Views\Company\List.cshtml"
                }
            }

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"clear\"></div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div" +
" class=\"grid1 marginTop_20 floatLeft\">\r\n    <div class=\"grid2 floatLeft\">\r\n     " +
"   <div id=\"taskRole\">\r\n            <ul>\r\n");


            
            #line 59 "..\..\Views\Company\List.cshtml"
                 if (Model.Companies.Count == 0)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <li>\r\n                        <div class=\"taskName\">No compan" +
"ies match the selected statuses</div>\r\n                    </li>\r\n");


            
            #line 64 "..\..\Views\Company\List.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                \r\n");


            
            #line 66 "..\..\Views\Company\List.cshtml"
                 foreach (var company in Model.Companies.OrderBy(x => x.Name))
                {

            
            #line default
            #line hidden
WriteLiteral("                    <li>\r\n                        <a href=\"");


            
            #line 69 "..\..\Views\Company\List.cshtml"
                            Write(Url.Action(MVC.Company.Details(company.Id)));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                            <div class=\"greyHighlight date\">");


            
            #line 70 "..\..\Views\Company\List.cshtml"
                                                       Write(company.LeadStatus);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                            <div class=\"taskName\">");


            
            #line 71 "..\..\Views\Company\List.cshtml"
                                             Write(company.Name);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                            <div class=\"taskDescription\">");


            
            #line 72 "..\..\Views\Company\List.cshtml"
                                                    Write(Html.ShortString(company.Notes, 85));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                        </a>\r\n                    </li>\r\n");


            
            #line 75 "..\..\Views\Company\List.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </ul>\r\n        </div>\r\n    </div>\r\n</div>");


        }
    }
}
#pragma warning restore 1591
