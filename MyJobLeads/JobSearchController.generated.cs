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
    public partial class JobSearchController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected JobSearchController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Edit() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Details() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Details);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Search() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Search);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public JobSearchController Actions { get { return MVC.JobSearch; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "JobSearch";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Add = "Add";
            public readonly string Edit = "Edit";
            public readonly string Details = "Details";
            public readonly string Search = "Search";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Details = "~/Views/JobSearch/Details.cshtml";
            public readonly string Edit = "~/Views/JobSearch/Edit.cshtml";
            public readonly string Index = "~/Views/JobSearch/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_JobSearchController: MyJobLeads.Controllers.JobSearchController {
        public T4MVC_JobSearchController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Add() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Add);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(int id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(MyJobLeads.DomainModel.Entities.JobSearch jobSearch) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            callInfo.RouteValueDictionary.Add("jobSearch", jobSearch);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Details(int id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Details);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Search(string searchTerm) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Search);
            callInfo.RouteValueDictionary.Add("searchTerm", searchTerm);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
