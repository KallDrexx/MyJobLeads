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
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using MyJobLeads.Infrastructure.HtmlHelpers;
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.1.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Company/Edit.cshtml")]
    public class Edit : System.Web.Mvc.WebViewPage<MyJobLeads.ViewModels.Companies.EditCompanyViewModel>
    {
        public Edit()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"lineSeperater\"> \r\n        <div " +
"class=\"pageInfoBox\"> \r\n");


            
            #line 6 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
             using (Html.BeginForm(MVC.Company.Edit())) 
            {
                
            
            #line default
            #line hidden
            
            #line 8 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.HiddenFor(x => x.JobSearchId));

            
            #line default
            #line hidden
            
            #line 8 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                                   
                
            
            #line default
            #line hidden
            
            #line 9 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
           Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
            
            #line 9 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                          


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n      " +
"                  <h3 class=\"floatLeft\">Edit  Company</h3> \r\n                </d" +
"iv> \r\n");


            
            #line 14 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Name:</p>\r\n                  " +
"      <div class=\"infoSpan\">");


            
            #line 17 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.TextBoxFor(x => x.Name, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n\r\n                    <div class=\"floatLeft\"" +
"><p class=\"greyHighlight\">Phone:</p>\r\n                        <div class=\"infoSp" +
"an\">");


            
            #line 21 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.TextBoxFor(x => x.Phone, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 24 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">City:</p>\r\n                  " +
"      <div class=\"infoSpan\">");


            
            #line 27 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.TextBoxFor(x => x.City, new { @class = "info "}));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n\r\n                    <div class=\"floatLeft\"" +
"><p class=\"greyHighlight\">State:</p>\r\n                        <div class=\"infoSp" +
"an\">");


            
            #line 31 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.TextBoxFor(x => x.State, new { @class = "info" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n\r\n                    <div class=\"floatLeft\"" +
"><p class=\"greyHighlight\">Zip:</p>\r\n                        <div class=\"infoSpan" +
"\">");


            
            #line 35 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.TextBoxFor(x => x.Zip, new { @class = "smallerInput" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 38 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Status:</p>\r\n                " +
"        <div class=\"infoSpan\">");


            
            #line 41 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.DropDownListFor(x => x.LeadStatus, new SelectList(Model.AvailableLeadStatuses, Model.LeadStatus)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div>                            \r\n                <" +
"/div> \r\n");


            
            #line 44 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
 

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                 " +
"   <div class=\"floatLeft\"><p class=\"greyHighlight\">Notes:</p>\r\n                 " +
"       <div class=\"infoSpan\">");


            
            #line 47 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                         Write(Html.TextAreaFor(x => x.Notes, new { @class = "textAreaInfo" }));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </div> \r\n                </div> \r\n");


            
            #line 50 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                 " +
"   <div class=\"submitBTN \">\r\n                        <input type=\"submit\" value=" +
"\"Save\" /> &nbsp;\r\n");


            
            #line 54 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                         if (Model.Id != 0)
                        {
                            
            
            #line default
            #line hidden
            
            #line 56 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                       Write(Html.ActionLink("Cancel", MVC.Company.Details(Model.Id)));

            
            #line default
            #line hidden
            
            #line 56 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                                                                     
                        }

                        else
                        {
                            
            
            #line default
            #line hidden
            
            #line 61 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                       Write(Html.ActionLink("Cancel", MVC.Company.List()));

            
            #line default
            #line hidden
            
            #line 61 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
                                                                          
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </div>                    \r\n                </div> \r\n");


            
            #line 65 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Company\Edit.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div class=\"clear\"></div> \r\n        </div> \r\n    </div> \r\n</div> ");


        }
    }
}
#pragma warning restore 1591
