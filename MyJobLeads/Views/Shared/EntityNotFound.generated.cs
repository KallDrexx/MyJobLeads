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

namespace MyJobLeads.Views.Shared
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EntityNotFound.cshtml")]
    public class EntityNotFound : System.Web.Mvc.WebViewPage<dynamic>
    {
        public EntityNotFound()
        {
        }
        public override void Execute()
        {

            
            #line 1 "..\..\Views\Shared\EntityNotFound.cshtml"
  
    string entityType = ViewBag.EntityType ?? "Entity";
    ViewBag.Title = entityType + " Not Found";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"lineSeperater\"> \r\n        <div " +
"class=\"pageInfoBox\"> \r\n            <div class=\"grid3 marginBottom_10 marginAuto " +
"floatLeft\"> \r\n                <h3 class=\"floatLeft\">");


            
            #line 10 "..\..\Views\Shared\EntityNotFound.cshtml"
                                 Write(entityType);

            
            #line default
            #line hidden
WriteLiteral(" Not Found</h3> \r\n            </div> \r\n                \r\n            <div class=\"" +
"grid3 marginBottom_10 marginAuto floatLeft\">\r\n                <div class=\"floatL" +
"eft\">\r\n                    <p>\r\n                        The ");


            
            #line 16 "..\..\Views\Shared\EntityNotFound.cshtml"
                       Write(entityType.ToLower());

            
            #line default
            #line hidden
WriteLiteral(" you requested could not be retrieved.  This could be due to the specified ");


            
            #line 16 "..\..\Views\Shared\EntityNotFound.cshtml"
                                                                                                                       Write(entityType.ToLower());

            
            #line default
            #line hidden
WriteLiteral("\r\n                        not existing, or your account not having access to the " +
"");


            
            #line 17 "..\..\Views\Shared\EntityNotFound.cshtml"
                                                                          Write(entityType.ToLower());

            
            #line default
            #line hidden
WriteLiteral(".\r\n                    </p>    \r\n                </div>\r\n            </div>\r\n\r\n  " +
"          <div class=\"clear\"></div> \r\n        </div> \r\n    </div> \r\n</div> \r\n");


        }
    }
}
#pragma warning restore 1591
