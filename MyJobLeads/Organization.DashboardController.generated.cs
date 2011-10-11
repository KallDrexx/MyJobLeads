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
namespace MyJobLeads.Areas.Organization.Controllers {
    public partial class DashboardController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected DashboardController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult HideDocumentFromMembers() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.HideDocumentFromMembers);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult ShowDocumentToMembers() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.ShowDocumentToMembers);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DashboardController Actions { get { return MVC.Organization.Dashboard; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Organization";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Dashboard";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string HideDocumentFromMembers = "HideDocumentFromMembers";
            public readonly string ShowDocumentToMembers = "ShowDocumentToMembers";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Index = "~/Areas/Organization/Views/Dashboard/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_DashboardController: MyJobLeads.Areas.Organization.Controllers.DashboardController {
        public T4MVC_DashboardController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult HideDocumentFromMembers(int docId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.HideDocumentFromMembers);
            callInfo.RouteValueDictionary.Add("docId", docId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult ShowDocumentToMembers(int docId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.ShowDocumentToMembers);
            callInfo.RouteValueDictionary.Add("docId", docId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
