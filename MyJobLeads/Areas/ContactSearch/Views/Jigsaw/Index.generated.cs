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

namespace MyJobLeads.Areas.ContactSearch.Views.Jigsaw
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
    
    #line 1 "..\..\Areas\ContactSearch\Views\Jigsaw\Index.cshtml"
    using MyJobLeads.Areas.ContactSearch.Models;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/ContactSearch/Views/Jigsaw/Index.cshtml")]
    public class Index : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Index()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\ContactSearch\Views\Jigsaw\Index.cshtml"
  
    ViewBag.Title = "Jigsaw Contact Search";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


DefineSection("SideBar", () => {

WriteLiteral("\r\n");


            
            #line 8 "..\..\Areas\ContactSearch\Views\Jigsaw\Index.cshtml"
      Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.ResearchCenter));

            
            #line default
            #line hidden

});

WriteLiteral("\r\n\r\n");


            
            #line 11 "..\..\Areas\ContactSearch\Views\Jigsaw\Index.cshtml"
  Html.RenderPartial(MVC.ContactSearch.Jigsaw.Views._SearchForm, new JigsawSearchParametersViewModel()); 
            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591