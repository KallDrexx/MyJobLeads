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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/CompanySearch/Views/Jigsaw/AddCompanySuccess.cshtml")]
    public class AddCompanySuccess : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.CompanySearch.Models.Jigsaw.AddCompanySuccessViewModel>
    {
        public AddCompanySuccess()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\CompanySearch\Views\Jigsaw\AddCompanySuccess.cshtml"
  
    ViewBag.Title = "Jigsaw Company Added Successfully";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "..\..\Areas\CompanySearch\Views\Jigsaw\AddCompanySuccess.cshtml"
 using (Html.PageInfoBox(false))
{
    using (Html.OuterRow())
    {
        using (Html.ContentArea())
        {

            
            #line default
            #line hidden
WriteLiteral("            <h3>Company Successfully Added From Jigsaw</h3>\r\n");



WriteLiteral("            <p>\r\n                You have successfully created ");


            
            #line 15 "..\..\Areas\CompanySearch\Views\Jigsaw\AddCompanySuccess.cshtml"
                                         Write(Html.ActionLink(Model.CompanyName, MVC.Company.Details(Model.CompanyId), new { @class = "inlineBlue" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                from data in the Jigsaw database.\r\n            </p>\r\n");


            
            #line 18 "..\..\Areas\CompanySearch\Views\Jigsaw\AddCompanySuccess.cshtml"
        }
    }
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
