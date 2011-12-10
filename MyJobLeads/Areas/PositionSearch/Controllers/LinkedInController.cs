using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.Areas.PositionSearch.Controllers
{
    [RequiresActiveJobSearch]
    public partial class LinkedInController : MyJobLeadsBaseController
    {
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _verifyTokenProcess;
        protected IProcess<StartLinkedInUserAuthParams, GeneralSuccessResultViewModel> _startLiAuthProcess;
        protected IProcess<ProcessLinkedInAuthProcessParams, GeneralSuccessResultViewModel> _finishLiAuthProcess;
        

        public LinkedInController(MyJobLeadsDbContext context,
                                    IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> verifyTokenProcess,
                                    IProcess<StartLinkedInUserAuthParams, GeneralSuccessResultViewModel> startLiAuthProcess,
                                    IProcess<ProcessLinkedInAuthProcessParams, GeneralSuccessResultViewModel> finishLiAuthProcess)
        {
            _context = context;
            _verifyTokenProcess = verifyTokenProcess;
            _startLiAuthProcess = startLiAuthProcess;
            _finishLiAuthProcess = finishLiAuthProcess;
        }

        public virtual ActionResult Index()
        {
            // If the user doesn't have a valid LinkedIn OAuth token, redirect them to get it
            if (!_verifyTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = CurrentUserId }).AccessTokenValid)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.AuthorizationAlert());

            return View();
        }

        public virtual ActionResult AuthorizationAlert()
        {
            return View();
        }

        public virtual ActionResult BeginAuthorization()
        {
            _startLiAuthProcess.Execute(new StartLinkedInUserAuthParams
            {
                ReturnUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action(MVC.PositionSearch.LinkedIn.ProcessAuthorization()))
            });

            // Shouldn't be hit
            return null;
        }

        public virtual ActionResult ProcessAuthorization()
        {
            var result = _finishLiAuthProcess.Execute(new ProcessLinkedInAuthProcessParams
            {
                UserId = CurrentUserId
            });

            if (result.WasSuccessful)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.Index());

            return View();
        }

    }
}
