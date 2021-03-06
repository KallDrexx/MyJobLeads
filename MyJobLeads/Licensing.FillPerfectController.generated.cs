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
namespace MyJobLeads.Areas.Licensing.Controllers {
    public partial class FillPerfectController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected FillPerfectController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.JsonResult Activate() {
            return new T4MVC_JsonResult(Area, Name, ActionNames.Activate);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.JsonResult GetLicense() {
            return new T4MVC_JsonResult(Area, Name, ActionNames.GetLicense);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public FillPerfectController Actions { get { return MVC.Licensing.FillPerfect; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Licensing";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "FillPerfect";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Activate = "Activate";
            public readonly string GetLicense = "GetLicense";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_FillPerfectController: MyJobLeads.Areas.Licensing.Controllers.FillPerfectController {
        public T4MVC_FillPerfectController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.JsonResult Activate(string key, string machineId) {
            var callInfo = new T4MVC_JsonResult(Area, Name, ActionNames.Activate);
            callInfo.RouteValueDictionary.Add("key", key);
            callInfo.RouteValueDictionary.Add("machineId", machineId);
            return callInfo;
        }

        public override System.Web.Mvc.JsonResult GetLicense(string key) {
            var callInfo = new T4MVC_JsonResult(Area, Name, ActionNames.GetLicense);
            callInfo.RouteValueDictionary.Add("key", key);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
