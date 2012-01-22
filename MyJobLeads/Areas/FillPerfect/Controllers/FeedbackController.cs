using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Areas.FillPerfect.Models.Surveys;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    public class FeedbackController : MyJobLeadsBaseController
    {
        public FeedbackController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public ActionResult SurveyCompleted(bool previouslyCompleted = false)
        {
            return View(new SurveyCompletedViewModel { SurveyPreviouslyCompleted = previouslyCompleted });
        }

        public ActionResult Student01(string fpUserId)
        {
            var model = new StudentSurvey01ViewModel { FpUserId = fpUserId };

            return View();
        }
    }
}
