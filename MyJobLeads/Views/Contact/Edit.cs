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

namespace MyJobLeads.Views.Contact
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
    
    #line 2 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
    using MyJobLeads.ViewModels.Contacts;
    
    #line default
    #line hidden
    using Telerik.Web.Mvc.UI;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.1.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Contact/Edit.cshtml")]
    public class Edit : System.Web.Mvc.WebViewPage<EditContactViewModel>
    {
        public Edit()
        {
        }
        public override void Execute()
        {


WriteLiteral("\r\n");


            
            #line 4 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";
    
    // Make sure the contact is at least initialized to an empty contact
    if (Model.Contact == null) { Model.Contact = new MyJobLeads.DomainModel.Entities.Contact(); }


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 11 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
 if (Model.Contact.Id == 0)
{
    ViewBag.Title = "Add A Contact";

            
            #line default
            #line hidden
WriteLiteral("    <h2>Add A Contact</h2>\r\n");



WriteLiteral("    <div class=\"ScreenHint\">This screen allows you to add a contact for ");


            
            #line 15 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
                                                                   Write(Model.Company.Name);

            
            #line default
            #line hidden
WriteLiteral(".</div>\r\n");


            
            #line 16 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
}
else
{
    ViewBag.Title = "Editing " + @Model.Contact.Name;

            
            #line default
            #line hidden
WriteLiteral("    <h2>Editing ");


            
            #line 20 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Model.Contact.Name);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");



WriteLiteral("    <div class=\"ScreenHint\">This screen allows you to edit the details for ");


            
            #line 21 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
                                                                      Write(Model.Contact.Name);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");


            
            #line 22 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<br />\r\n\r\n");


            
            #line 26 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
   int companyId = Model.Company.Id; 

            
            #line default
            #line hidden

            
            #line 27 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
 using (Html.BeginForm(MVC.Contact.Edit(Model.Contact.Id)))
{
    
            
            #line default
            #line hidden
            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
Write(Html.HiddenFor(x => x.Company.Id));

            
            #line default
            #line hidden
            
            #line 29 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
                                      
    
            
            #line default
            #line hidden
            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
Write(Html.HiddenFor(x => x.Contact.Id));

            
            #line default
            #line hidden
            
            #line 30 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
                                      


            
            #line default
            #line hidden
WriteLiteral("    <p>");


            
            #line 32 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
  Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");


            
            #line 33 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <table>\r\n        <tr>\r\n            <td align=\"right\">Name:</td>\r\n            " +
"<td>");


            
            #line 37 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.Name));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Direct Phone:" +
"</td>\r\n            <td>");


            
            #line 42 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.DirectPhone));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Extension:</t" +
"d>\r\n            <td>");


            
            #line 47 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.Extension));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Mobile Phone:" +
"</td>\r\n            <td>");


            
            #line 52 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.MobilePhone));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Email:</td>\r\n" +
"            <td>");


            
            #line 57 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.Email));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Assistant:</t" +
"d>\r\n            <td>");


            
            #line 62 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.Assistant));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\">Referred By:<" +
"/td>\r\n            <td>");


            
            #line 67 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.EditorFor(x => x.Contact.ReferredBy));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td align=\"right\" valign=\"top\">" +
"Notes:</td>\r\n            <td>");


            
            #line 72 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
           Write(Html.TextAreaFor(x => x.Contact.Notes, new { rows = 8, cols = 50 }));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        </tr>\r\n\r\n        <tr>\r\n            <td></td>\r\n            <td>\r\n  " +
"              <input type=\"submit\" value=\"Save\" /> &nbsp; &nbsp;\r\n");


            
            #line 79 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
                 if (Model.Contact.Id == 0) { Html.ActionLink("Back", MVC.Company.Details(companyId)); }
                else { Html.ActionLink("Back", MVC.Contact.Details(Model.Contact.Id)); }

            
            #line default
            #line hidden
WriteLiteral("            </td>\r\n        </tr>\r\n    </table>\r\n");


            
            #line 84 "C:\Users\KallDrexx\Documents\Projects\MyJobLeads\MyJobLeads\Views\Contact\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");


        }
    }
}
#pragma warning restore 1591
