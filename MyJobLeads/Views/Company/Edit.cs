﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
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
    using System.Web.Mvc.Html;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Company/Edit.cshtml")]
    public class Edit : System.Web.Mvc.WebViewPage<MyJobLeads.DomainModel.Entities.Company>
    {
        public Edit()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
 if (Model.Id == 0)
{
    ViewBag.Title = "Add A Company";

            
            #line default
            #line hidden
WriteLiteral("    <h2>Add A Company</h2>\r\n");



WriteLiteral("    <div class=\"ScreenHint\">This screen allows you to add a company to your job s" +
"earch.</div>\r\n");


            
            #line 12 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
}
else
{
    ViewBag.Title = "Editing " + @Model.Name;

            
            #line default
            #line hidden
WriteLiteral("    <h2>Editing ");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");



WriteLiteral("    <div class=\"ScreenHint\">This screen allows you to edit the ");


            
            #line 17 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                                          Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral(" company.</div>\r\n");


            
            #line 18 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("<br />\r\n\r\n");


            
            #line 21 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
 using (Html.BeginForm(MVC.Company.Edit(Model.Id)))
{
    
            
            #line default
            #line hidden
            
            #line 23 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
Write(Html.Hidden("jobSearchId", (int)ViewBag.JobSearchId));

            
            #line default
            #line hidden
            
            #line 23 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                                         
    
            
            #line default
            #line hidden
            
            #line 24 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
            
            #line 24 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                              


            
            #line default
            #line hidden
WriteLiteral("    <table>\r\n        <tr>\r\n            <td align=\"right\">Name:</td>\r\n            " +
"<td>");


            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.EditorFor(x => x.Name));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Phone Number:" +
"</td>\r\n            <td>");


            
            #line 34 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.EditorFor(x => x.Phone));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">City:</td>\r\n " +
"           <td>");


            
            #line 39 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.EditorFor(x => x.City));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">State:</td>\r\n" +
"            <td>");


            
            #line 44 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.EditorFor(x => x.State));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Zip:</td>\r\n  " +
"          <td>");


            
            #line 49 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.EditorFor(x => x.Zip));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\" valign=\"top\">" +
"Notes:</td>\r\n            <td>");


            
            #line 54 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.TextAreaFor(x => x.Notes, new { rows = 10, cols = 50 }));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td></td>\r\n            <td>\r\n  " +
"              <input type=\"submit\" value=\"Save\" />\r\n            </td>\r\n        <" +
"/tr>\r\n    </table>\r\n");


            
            #line 64 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");


        }
    }
}
#pragma warning restore 1591