﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Areas.FillPerfect.Models.Surveys;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    public partial class FeedbackController : MyJobLeadsBaseController
    {
        public FeedbackController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult SurveyCompleted(bool previouslyCompleted = false)
        {
            return View(new SurveyCompletedViewModel { SurveyPreviouslyCompleted = previouslyCompleted });
        }

        public virtual ActionResult Student01(string fpUserId)
        {
            if (string.IsNullOrWhiteSpace(fpUserId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(true));

            var model = new StudentSurvey01ViewModel { FpUserId = fpUserId };
            if (_context.FpSurveyResponses.Any(x => x.FpUserId == fpUserId && x.SurveyId == model.QuestionId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(true));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student01(StudentSurvey01ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.GetSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted());
        }
    }
}
