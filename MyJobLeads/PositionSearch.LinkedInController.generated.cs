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
namespace MyJobLeads.Areas.PositionSearch.Controllers {
    public partial class LinkedInController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected LinkedInController(Dummy d) { }

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
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult PositionDetails() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.PositionDetails);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult AddPosition() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.AddPosition);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult AddPositionSuccessful() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.AddPositionSuccessful);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public LinkedInController Actions { get { return MVC.PositionSearch.LinkedIn; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "PositionSearch";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "LinkedIn";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string PerformSearch = "PerformSearch";
            public readonly string PositionDetails = "PositionDetails";
            public readonly string AddPosition = "AddPosition";
            public readonly string AddPositionSuccessful = "AddPositionSuccessful";
            public readonly string AuthorizationAlert = "AuthorizationAlert";
            public readonly string BeginAuthorization = "BeginAuthorization";
            public readonly string ProcessAuthorization = "ProcessAuthorization";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string _SearchForm = "~/Areas/PositionSearch/Views/LinkedIn/_SearchForm.cshtml";
            public readonly string AddPosition = "~/Areas/PositionSearch/Views/LinkedIn/AddPosition.cshtml";
            public readonly string AddPositionSuccessful = "~/Areas/PositionSearch/Views/LinkedIn/AddPositionSuccessful.cshtml";
            public readonly string AuthorizationAlert = "~/Areas/PositionSearch/Views/LinkedIn/AuthorizationAlert.cshtml";
            public readonly string Index = "~/Areas/PositionSearch/Views/LinkedIn/Index.cshtml";
            public readonly string PerformSearch = "~/Areas/PositionSearch/Views/LinkedIn/PerformSearch.cshtml";
            public readonly string PositionDetails = "~/Areas/PositionSearch/Views/LinkedIn/PositionDetails.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_LinkedInController: MyJobLeads.Areas.PositionSearch.Controllers.LinkedInController {
        public T4MVC_LinkedInController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult PerformSearch(MyJobLeads.Areas.PositionSearch.Models.PositionSearchQueryViewModel searchParams, int pageNum) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.PerformSearch);
            callInfo.RouteValueDictionary.Add("searchParams", searchParams);
            callInfo.RouteValueDictionary.Add("pageNum", pageNum);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult PositionDetails(string id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.PositionDetails);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult AddPosition(string positionId, string positionTitle, string companyName) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.AddPosition);
            callInfo.RouteValueDictionary.Add("positionId", positionId);
            callInfo.RouteValueDictionary.Add("positionTitle", positionTitle);
            callInfo.RouteValueDictionary.Add("companyName", companyName);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult AddPosition(MyJobLeads.Areas.PositionSearch.Models.AddPositionViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.AddPosition);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult AddPositionSuccessful(int companyId, string companyName, int positionId, string positionTitle) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.AddPositionSuccessful);
            callInfo.RouteValueDictionary.Add("companyId", companyId);
            callInfo.RouteValueDictionary.Add("companyName", companyName);
            callInfo.RouteValueDictionary.Add("positionId", positionId);
            callInfo.RouteValueDictionary.Add("positionTitle", positionTitle);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult AuthorizationAlert() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.AuthorizationAlert);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult BeginAuthorization() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.BeginAuthorization);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult ProcessAuthorization() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.ProcessAuthorization);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591