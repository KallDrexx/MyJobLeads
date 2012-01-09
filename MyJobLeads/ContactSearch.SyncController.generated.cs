// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MyJobLeads.Areas.ContactSearch.Controllers {
    public partial class SyncController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected SyncController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Jigsaw() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Jigsaw);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public SyncController Actions { get { return MVC.ContactSearch.Sync; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "ContactSearch";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Sync";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Jigsaw = "Jigsaw";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Jigsaw = "~/Areas/ContactSearch/Views/Sync/Jigsaw.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_SyncController: MyJobLeads.Areas.ContactSearch.Controllers.SyncController {
        public T4MVC_SyncController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Jigsaw(int contactId, int jigsawId, string externalName, string externalTitle, System.DateTime lastUpdated, string externalEmail, string externalPhone) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Jigsaw);
            callInfo.RouteValueDictionary.Add("contactId", contactId);
            callInfo.RouteValueDictionary.Add("jigsawId", jigsawId);
            callInfo.RouteValueDictionary.Add("externalName", externalName);
            callInfo.RouteValueDictionary.Add("externalTitle", externalTitle);
            callInfo.RouteValueDictionary.Add("lastUpdated", lastUpdated);
            callInfo.RouteValueDictionary.Add("externalEmail", externalEmail);
            callInfo.RouteValueDictionary.Add("externalPhone", externalPhone);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Jigsaw(MyJobLeads.Areas.ContactSearch.Models.Sync.JigsawSyncViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Jigsaw);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
