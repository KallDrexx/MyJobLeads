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
    public partial class JigsawController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected JigsawController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult PerformSearch() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.PerformSearch);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public JigsawController Actions { get { return MVC.ContactSearch.Jigsaw; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "ContactSearch";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Jigsaw";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Authenticate = "Authenticate";
            public readonly string PerformSearch = "PerformSearch";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string _SearchForm = "~/Areas/ContactSearch/Views/Jigsaw/_SearchForm.cshtml";
            public readonly string Authenticate = "~/Areas/ContactSearch/Views/Jigsaw/Authenticate.cshtml";
            public readonly string Index = "~/Areas/ContactSearch/Views/Jigsaw/Index.cshtml";
            public readonly string PerformSearch = "~/Areas/ContactSearch/Views/Jigsaw/PerformSearch.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_JigsawController: MyJobLeads.Areas.ContactSearch.Controllers.JigsawController {
        public T4MVC_JigsawController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Authenticate(bool loginFailed) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Authenticate);
            callInfo.RouteValueDictionary.Add("loginFailed", loginFailed);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Authenticate(MyJobLeads.Areas.ContactSearch.Models.JigsawAuthenticateViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Authenticate);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult PerformSearch(MyJobLeads.Areas.ContactSearch.Models.JigsawSearchParametersViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.PerformSearch);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
