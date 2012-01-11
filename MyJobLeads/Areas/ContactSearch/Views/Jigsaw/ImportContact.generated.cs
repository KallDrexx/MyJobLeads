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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/ContactSearch/Views/Jigsaw/ImportContact.cshtml")]
    public class ImportContact : System.Web.Mvc.WebViewPage<MyJobLeads.Areas.ContactSearch.Models.Jigsaw.ImportContactViewModel>
    {
        public ImportContact()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
  
    ViewBag.Title = "Import Jigsaw Contact";
    Html.Telerik().ScriptRegistrar().OnDocumentReady(@"
        // Set defaults
        hideNewContact();

        $('#CreateNewContact').change(function() {
            if ($(this).is(':checked')) { showNewContact(); }
            else { hideNewContact(); }
        });

        if ($('#CreateNewContact').is(':checked')) { showNewContact(); }
        "
    );


            
            #line default
            #line hidden
WriteLiteral(@"
<script type=""text/javascript"">
    var newCompanyDiv = '#newCompanyDiv';
    var existingCompanyDiv = '#existingCompanyDiv';
    var existingContactDiv = '#existingContactDiv';

    function showNewContact() {
        $(existingContactDiv).hide();
        $(newCompanyDiv).show();
        $(existingCompanyDiv).show();
    }

    function hideNewContact() {
        $(existingContactDiv).show();
        $(newCompanyDiv).hide();
        $(existingCompanyDiv).hide();
    }
</script>

<div class=""grid1 floatLeft""> 
    <div class=""pageInfoBox""> 
");


            
            #line 39 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
         using (Html.BeginForm()) 
        {
            
            
            #line default
            #line hidden
            
            #line 41 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
       Write(Html.HiddenFor(x => x.JigsawContactId));

            
            #line default
            #line hidden
            
            #line 41 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                                   
            
            
            #line default
            #line hidden
            
            #line 42 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
       Write(Html.HiddenFor(x => x.JigsawCompanyId));

            
            #line default
            #line hidden
            
            #line 42 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                                   
            
            
            #line default
            #line hidden
            
            #line 43 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
       Write(Html.HiddenFor(x => x.CompanyName));

            
            #line default
            #line hidden
            
            #line 43 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                               
            
            
            #line default
            #line hidden
            
            #line 44 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
       Write(Html.HiddenFor(x => x.ContactName));

            
            #line default
            #line hidden
            
            #line 44 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                               
            
            
            #line default
            #line hidden
            
            #line 45 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
       Write(Html.HiddenFor(x => x.ContactTitle));

            
            #line default
            #line hidden
            
            #line 45 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                                
            
            
            #line default
            #line hidden
            
            #line 46 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
       Write(Html.HiddenFor(x => x.JigsawUpdatedDate));

            
            #line default
            #line hidden
            
            #line 46 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                                     
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\"> \r\n          " +
"      <h3 class=\"floatLeft\">Import Contact</h3> \r\n            </div> \r\n");


            
            #line 51 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 marginAuto floatLeft"">
                <p>
                    Please review this contact's information, and choose whether to add it as a new contact or merge with an existing contact.
                </p>
            </div>
");


            
            #line 57 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatLeft\">\r\n           " +
"     <div class=\"floatLeft\">\r\n                    <p>\r\n                        N" +
"ame: <span class=\"bold\">");


            
            #line 61 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                            Write(Model.ContactName);

            
            #line default
            #line hidden
WriteLiteral("</span><br />\r\n                        Title: <span class=\"bold\">");


            
            #line 62 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                             Write(Model.ContactTitle);

            
            #line default
            #line hidden
WriteLiteral("</span><br />\r\n                        Company: <span class=\"bold\">");


            
            #line 63 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                               Write(Model.CompanyName);

            
            #line default
            #line hidden
WriteLiteral("</span><br />\r\n                        Source: <a href=\"http://www.jigsaw.com/BC." +
"xhtml?contactId=");


            
            #line 64 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                                                             Write(Model.JigsawContactId);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"inlineBlue\">Jigsaw</a>\r\n                    </p>    \r\n                </" +
"div>\r\n            </div>\r\n");


            
            #line 68 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 marginAuto floatleft\">\r\n           " +
"     <div class=\"floatLeft infoSpan\">\r\n                    ");


            
            #line 71 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
               Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n");


            
            #line 74 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\"> \r\n                <div " +
"class=\"floatLeft\">\r\n                    <p class=\"greyHighlight\">");


            
            #line 77 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                        Write(Html.LabelFor(x => x.CreateNewContact, "Create New Contact Entry For " + Model.ContactName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        ");


            
            #line 78 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                   Write(Html.CheckBoxFor(x => x.CreateNewContact));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </p>\r\n                </div> \r\n            </div> \r\n");


            
            #line 82 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"


            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 floatLeft"" id=""existingContactDiv""> 
                <div class=""floatLeft"">
                    <p class=""greyHighlight""><span class=""bold"">OR</span></p>
                    <p class=""greyHighlight"">Merge with existing contact:</p>
                    <div class=""infoSpan"">
                        ");


            
            #line 88 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                   Write(Html.DropDownListFor(x => x.SelectedContactId, Model.ExistingContactList));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </div> \r\n            </div> \r\n");


            
            #line 92 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_10 floatLeft\" id=\"newCompanyDiv\"> \r\n  " +
"              <div class=\"floatLeft\">\r\n                    <p class=\"greyHighlig" +
"ht\">");


            
            #line 95 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                                        Write(Html.LabelFor(x => x.CreateNewCompany, "Create New Company Entry For " + Model.CompanyName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        ");


            
            #line 96 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                   Write(Html.CheckBoxFor(x => x.CreateNewCompany));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </p>\r\n                </div> \r\n            </div>\r\n");


            
            #line 100 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral(@"            <div class=""grid3 marginBottom_10 floatLeft"" id=""existingCompanyDiv""> 
                <div class=""floatLeft"">
                    <p class=""greyHighlight""><span class=""bold"">OR</span></p>
                    <p class=""greyHighlight"">Merge with an existing company:</p>
                    <div class=""infoSpan"">
                        ");


            
            #line 106 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
                   Write(Html.DropDownListFor(x => x.SelectedCompanyId, Model.ExistingCompanyList));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </div> \r\n            </div> \r\n");


            
            #line 110 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <div class=\"grid3 marginBottom_20 floatLeft\"> \r\n                <div " +
"class=\"submitBTN \">\r\n                    <input type=\"submit\" value=\"Import Cont" +
"act\" />\r\n                </div>                    \r\n            </div> \r\n");


            
            #line 116 "..\..\Areas\ContactSearch\Views\Jigsaw\ImportContact.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div class=\"clear\"></div> \r\n    </div> \r\n</div>");


        }
    }
}
#pragma warning restore 1591