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

namespace MyJobLeads.Areas.CompanySearch.Views.Jigsaw
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/CompanySearch/Views/Jigsaw/Sync.cshtml")]
    public class Sync : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.CompanySearch.Models.Jigsaw.SyncCompanyViewModel>
    {
        public Sync()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
  
    ViewBag.Title = "Sync Company With Jigsaw";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 8 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
      Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.ResearchCenter));

            
            #line default
            #line hidden

});

WriteLiteral("\r\n\r\n");


            
            #line 11 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
 using (Html.PageInfoBox(false))
{
    using (Html.BeginForm())
    {
        
            
            #line default
            #line hidden
            
            #line 15 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.CompanyId));

            
            #line default
            #line hidden
            
            #line 15 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                         
        
            
            #line default
            #line hidden
            
            #line 16 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawId));

            
            #line default
            #line hidden
            
            #line 16 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                        
        
            
            #line default
            #line hidden
            
            #line 17 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.InternalAddress));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                               
        
            
            #line default
            #line hidden
            
            #line 18 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.InternalIndustry));

            
            #line default
            #line hidden
            
            #line 18 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                                
        
            
            #line default
            #line hidden
            
            #line 19 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.InternalName));

            
            #line default
            #line hidden
            
            #line 19 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                            
        
            
            #line default
            #line hidden
            
            #line 20 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.InternalPhone));

            
            #line default
            #line hidden
            
            #line 20 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                             
        
            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.InternalWebsite));

            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                               
        
            
            #line default
            #line hidden
            
            #line 22 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawAddress));

            
            #line default
            #line hidden
            
            #line 22 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                             
        
            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawIndustry));

            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                              
        
            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawName));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                          
        
            
            #line default
            #line hidden
            
            #line 25 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawPhone));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                           
        
            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawEmployeeCount));

            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                                   
        
            
            #line default
            #line hidden
            
            #line 27 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawOwnership));

            
            #line default
            #line hidden
            
            #line 27 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                               
        
            
            #line default
            #line hidden
            
            #line 28 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawRevenue));

            
            #line default
            #line hidden
            
            #line 28 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                             
        
            
            #line default
            #line hidden
            
            #line 29 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.JigsawWebsite));

            
            #line default
            #line hidden
            
            #line 29 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                             
        
            
            #line default
            #line hidden
            
            #line 30 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
   Write(Html.HiddenFor(x => x.LastUpdatedOnJigaw));

            
            #line default
            #line hidden
            
            #line 30 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                                  
        
        using (Html.OuterRow())
        {

            
            #line default
            #line hidden
WriteLiteral("            <h3>Sync Company With Data From Jigsaw</h3>\r\n");


            
            #line 35 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
        }

        using (Html.OuterRow())
        {
            using (Html.ContentArea())
            {

            
            #line default
            #line hidden
WriteLiteral("                <p>\r\n                    This page allows you to select what data" +
" you wish to import from the internet for this particular company. <br />\r\n     " +
"               This company\'s data was last updated in Jigsaw\'s database on ");


            
            #line 43 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                                                            Write(Model.LastUpdatedOnJigaw.ToString("D"));

            
            #line default
            #line hidden
WriteLiteral(".\r\n                </p>\r\n");


            
            #line 45 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
            }
        }

        using (Html.OuterRow())
        {
            using (Html.ValidationArea())
            {
                
            
            #line default
            #line hidden
            
            #line 52 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
           Write(Html.ValidationSummary());

            
            #line default
            #line hidden
            
            #line 52 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                                         
            }
        }

        using (Html.OuterRow())
        {

            
            #line default
            #line hidden
WriteLiteral(@"            <table id=""SyncDataTable"" class=""display"">
                <thead>
                    <th></th>
                    <th>Import?</th>
                    <th>MyLeads Data</th>
                    <th>Jigsaw Data</th>
                </thead>

                <tr>
                    <td>Company Name:</td>
                    <td>");


            
            #line 68 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Html.CheckBoxFor(x => x.ImportName));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 69 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.InternalName);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 70 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.JigsawName);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                </tr>\r\n\r\n                <tr>\r\n                    <td>Add" +
"ress:</td>\r\n                    <td>");


            
            #line 75 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Html.CheckBoxFor(x => x.ImportAddress));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 76 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.InternalAddress);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 77 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.JigsawAddress);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                </tr>\r\n\r\n                <tr>\r\n                    <td>Pho" +
"ne:</td>\r\n                    <td>");


            
            #line 82 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Html.CheckBoxFor(x => x.ImportPhone));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 83 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.InternalPhone);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 84 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.JigsawPhone);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                </tr>\r\n\r\n                <tr>\r\n                    <td>Ind" +
"ustry:</td>\r\n                    <td>");


            
            #line 89 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Html.CheckBoxFor(x => x.ImportIndustry));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 90 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.InternalIndustry);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 91 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.JigsawIndustry);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                </tr>\r\n\r\n                <tr>\r\n                    <td>Web" +
"site:</td>\r\n                    <td>");


            
            #line 96 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Html.CheckBoxFor(x => x.ImportWebsite));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 97 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.InternalWebsite);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");


            
            #line 98 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
                   Write(Model.JigsawWebsite);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                </tr>\r\n            </table>\r\n");


            
            #line 101 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
        }

        using (Html.OuterRow())
        {
            using (Html.FormButtonArea())
            {

            
            #line default
            #line hidden
WriteLiteral("                <input type=\"submit\" value=\"Perform Sync\" />\r\n");


            
            #line 108 "..\..\Areas\CompanySearch\Views\Jigsaw\Sync.cshtml"
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