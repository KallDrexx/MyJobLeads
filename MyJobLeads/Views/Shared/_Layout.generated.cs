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
    
    #line 1 "..\..\Views\Shared\_Layout.cshtml"
    using Telerik.Web.Mvc.UI;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout.cshtml")]
    public class Layout : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Layout()
        {
        }
        public override void Execute()
        {

WriteLiteral(@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html lang=""en"">
<head>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />	
    <meta name=""description"" content=""Your description goes here"" />
	<meta name=""keywords"" content=""your,keywords,goes,here"" />
	<meta name=""author"" content=""Your Name"" />
    <title>MyLeads: ");


            
            #line 10 "..\..\Views\Shared\_Layout.cshtml"
               Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</title>\r\n    ");


            
            #line 11 "..\..\Views\Shared\_Layout.cshtml"
Write(Html.Telerik().StyleSheetRegistrar()
                    .DefaultGroup(group => group
                        .Add("~/Content/Site.css")
                        .Add("telerik.common.css")
                        .Add("Telerik.vista.css")
                        .Add("~/Scripts/plugins/DataTables/css/demo_table.css")
                        .Combined(true)
                        .Compress(true))
    );

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    ");



WriteLiteral(@"
    <script type=""text/javascript"">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-26207392-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

    </script>
</head>

<body>

<div id=""myLeadsheader""> 
		<div id=""headerWrap""> 
        	<div class=""leftColumn""> 
            	<h1 id=""myLogo"">");


            
            #line 42 "..\..\Views\Shared\_Layout.cshtml"
                        Write(Html.ActionLink("Portal", MVC.Home.Index()));

            
            #line default
            #line hidden
WriteLiteral("</h1> \r\n            </div> \r\n\r\n            <div class=\"rightColumn\"> \r\n          " +
"  \t<div id=\"headerNavWrapper\"> \r\n");


            
            #line 47 "..\..\Views\Shared\_Layout.cshtml"
                     if (User.Identity.IsAuthenticated)
                    {

            
            #line default
            #line hidden
WriteLiteral("                \t    <div id=\"headerSearch\"> \r\n");


            
            #line 50 "..\..\Views\Shared\_Layout.cshtml"
                             using (Html.BeginForm(MVC.JobSearch.Search()))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <input class=\"headerInput\" type=\"text\" name=\"quer" +
"y\" id=\"query\" value=\"");


            
            #line 52 "..\..\Views\Shared\_Layout.cshtml"
                                                                                                 Write(TempData["LastSearchQuery"]);

            
            #line default
            #line hidden
WriteLiteral("\" /> \r\n");



WriteLiteral("                                <input type=\"submit\" value=\"Search\" class=\"search" +
"Submit\" />\r\n");


            
            #line 54 "..\..\Views\Shared\_Layout.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </div> \r\n");


            
            #line 56 "..\..\Views\Shared\_Layout.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                       \r\n                    <div id=\"headerNav\"> \r\n             " +
"           <ul> \r\n                        <li>");


            
            #line 60 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("Home", MVC.Home.Index()));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");


            
            #line 61 "..\..\Views\Shared\_Layout.cshtml"
                         if (User.Identity.IsAuthenticated)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li>");


            
            #line 63 "..\..\Views\Shared\_Layout.cshtml"
                           Write(Html.ActionLink("Log Off", MVC.Account.LogOff()));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");


            
            #line 64 "..\..\Views\Shared\_Layout.cshtml"
                        }
                        else
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li>");


            
            #line 67 "..\..\Views\Shared\_Layout.cshtml"
                           Write(Html.ActionLink("Log On", MVC.Account.LogOn()));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");


            
            #line 68 "..\..\Views\Shared\_Layout.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        </ul> \r\n                    </div> \r\n                </di" +
"v> \r\n            </div>  \t\r\n        </div> \r\n</div> \r\n<div id=\"myLeadsBodyWrap\">" +
" \r\n \r\n\t\t<div class=\"leftColumn\"> \r\n");


            
            #line 78 "..\..\Views\Shared\_Layout.cshtml"
             if (IsSectionDefined("SideBar"))
            {
                
            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\Shared\_Layout.cshtml"
           Write(RenderSection("SideBar"));

            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\Shared\_Layout.cshtml"
                                         ;
            }
            else
            {
                Html.RenderAction(MVC.Home.SidebarDisplay(MyJobLeads.Controllers.ActiveSidebarLink.None));
            }

            
            #line default
            #line hidden
WriteLiteral("        </div> \r\n        <div class=\"rightColumn\"> \r\n            \t<div id=\"conten" +
"tWrapper\"> \r\n                    ");


            
            #line 89 "..\..\Views\Shared\_Layout.cshtml"
               Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral(@"                
                <div class=""clear""></div> 
                </div> 
                <div class=""footer"">
                    <div class=""grid2 floatLeft"">MyLeads © 2011</div>
                    <div class=""grid2 floatRight"">
                        <ul class=""footerLinks"">
                            <li>");


            
            #line 96 "..\..\Views\Shared\_Layout.cshtml"
                           Write(Html.ActionLink("Security", MVC.Policies.Security(), new { @title = "Security Policy" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                            <li>");


            
            #line 97 "..\..\Views\Shared\_Layout.cshtml"
                           Write(Html.ActionLink("Privacy", MVC.Policies.Privacy(), new { @title = "Privacy Policy" }));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                            <li><a href=\"http://interviewtools.net/\">Inter" +
"viewTools</a></li>\r\n                        </ul>\r\n                    </div>\r\n " +
"               </div> \r\n        </div> \r\n</div> \r\n\r\n");


            
            #line 105 "..\..\Views\Shared\_Layout.cshtml"
Write( Html.Telerik().ScriptRegistrar());

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n</body>\r\n</html>\r\n");


        }
    }
}
#pragma warning restore 1591
