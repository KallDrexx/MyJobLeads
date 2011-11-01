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
    public partial class MemberStatsController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected MemberStatsController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Index() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Index);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public MemberStatsController Actions { get { return MVC.Organization.MemberStats; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Organization";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "MemberStats";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_MemberStatsController: MyJobLeads.Areas.Organization.Controllers.MemberStatsController {
        public T4MVC_MemberStatsController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index(int organizationId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            callInfo.RouteValueDictionary.Add("organizationId", organizationId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
