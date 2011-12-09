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

namespace MyJobLeads.Areas.PositionSearch.Controllers
{
    [RequiresActiveJobSearch]
    public partial class LinkedInController : MyJobLeadsBaseController
    {
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _verifyTokenProcess;

        public LinkedInController(MyJobLeadsDbContext context,
                                    IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> verifyTokenProcess)
        {
            _context = context;
            _verifyTokenProcess = verifyTokenProcess;
        }

        public virtual ActionResult Index()
        {
            // If the user doesn't have a valid LinkedIn OAuth token, redirect them to get it
            if (!_verifyTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = CurrentUserId }).AccessTokenValid)
                return RedirectToAction(MVC.PositionSearch.LinkedIn.BeginAuthorization());

            return View();
        }

        public virtual ActionResult BeginAuthorization()
        {
            return View();
        }

    }
}
