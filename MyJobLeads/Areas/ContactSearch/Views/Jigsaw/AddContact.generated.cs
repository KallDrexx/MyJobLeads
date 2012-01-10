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
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/ContactSearch/Views/Jigsaw/AddContact.cshtml")]
    public class AddContact : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.ContactSearch.Models.AddJigsawContactViewModel>
    {
        public AddContact()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
  
    ViewBag.Title = "Add Jigsaw Contact";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"grid1 floatLeft\"> \r\n    <div class=\"pageInfoBox\"> \r\n");


            
            #line 9 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
         using (Html.BeginForm()) 
        {
            
            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
       Write(Html.HiddenFor(x => x.JigsawContactId));

            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                                   
            
            
            #line default
            #line hidden
            
            #line 12 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
       Write(Html.HiddenFor(x => x.JigsawCompanyId));

            
            #line default
            #line hidden
            
            #line 12 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                                   
            
            
            #line default
            #line hidden
            
            #line 13 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
       Write(Html.HiddenFor(x => x.JigsawCompanyName));

            
            #line default
            #line hidden
            
            #line 13 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                                     
            
            
            #line default
            #line hidden
            
            #line 14 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
       Write(Html.HiddenFor(x => x.Name));

            
            #line default
            #line hidden
            
            #line 14 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                        
            
            
            #line default
            #line hidden
            
            #line 15 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
       Write(Html.HiddenFor(x => x.Title));

            
            #line default
            #line hidden
            
            #line 15 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                         
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n          " +
"      <h3 class=\"floatLeft\">What Company Do You Wish To Add This Contact To?</h3" +
"> \r\n            </div> \r\n");


            
            #line 20 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n           " +
"     <div class=\"floatLeft\">\r\n                    <p>\r\n                        N" +
"ame: <span class=\"bold\">");


            
            #line 24 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                            Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</span><br />\r\n                        Company: <span class=\"bold\">");


            
            #line 25 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                               Write(Model.JigsawCompanyName);

            
            #line default
            #line hidden
WriteLiteral("</span><br />\r\n                        Source: <a href=\"http://www.jigsaw.com/BC." +
"xhtml?contactId=");


            
            #line 26 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                                                             Write(Model.JigsawContactId);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"inlineBlue\">Jigsaw</a>\r\n                    </p>    \r\n                </" +
"div>\r\n            </div>\r\n");


            
            #line 30 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatleft\">\r\n           " +
"     <div class=\"floatLeft infoSpan\">\r\n                    ");


            
            #line 33 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
               Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n");


            
            #line 36 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\">\r\n                    <p class=\"greyHighlight\">");


            
            #line 39 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                                        Write(Html.LabelFor(x => x.CreateNewCompany, "Create New Company Entry For " + Model.JigsawCompanyName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        ");


            
            #line 40 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                   Write(Html.CheckBoxFor(x => x.CreateNewCompany));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </p>\r\n                </div> \r\n            </div> \r\n");


            
            #line 44 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"


            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 floatLeft""> 
                <div class=""floatLeft"">
                    <p class=""greyHighlight""><span class=""bold"">OR</span></p>
                    <p class=""greyHighlight"">Add Position To Existing Company:</p>
                    <div class=""infoSpan"">
                        ");


            
            #line 50 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
                   Write(Html.DropDownListFor(x => x.SelectedCompanyId, Model.ExistingCompanyList));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </div> \r\n            </div> \r\n");


            
            #line 54 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                <div " +
"class=\"submitBTN \">\r\n                    <input type=\"submit\" value=\"Add Contact" +
"\" />\r\n                </div>                    \r\n            </div> \r\n");


            
            #line 60 "..\..\Areas\ContactSearch\Views\Jigsaw\AddContact.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div class=\"clear\"></div> \r\n    </div> \r\n</div> \r\n");


        }
    }
}
#pragma warning restore 1591
