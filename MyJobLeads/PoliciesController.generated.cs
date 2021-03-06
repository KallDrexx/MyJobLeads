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
namespace MyJobLeads.Controllers {
    public partial class PoliciesController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PoliciesController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected PoliciesController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }


        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PoliciesController Actions { get { return MVC.Policies; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Policies";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Privacy = "Privacy";
            public readonly string Security = "Security";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Privacy = "~/Views/Policies/Privacy.cshtml";
            public readonly string Security = "~/Views/Policies/Security.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_PoliciesController: MyJobLeads.Controllers.PoliciesController {
        public T4MVC_PoliciesController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Privacy() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Privacy);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Security() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Security);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
