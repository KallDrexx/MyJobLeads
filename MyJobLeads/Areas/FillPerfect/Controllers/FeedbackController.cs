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
            var model = new StudentSurveyPost3ViewModel { FpUserId = fpUserId };
            if (string.IsNullOrWhiteSpace(fpUserId) || _context.FpSurveyResponses.Any(x => x.FpUserId == fpUserId && x.SurveyId == model.QuestionId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(true));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student01(StudentSurveyPost3ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.CreateSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted());
        }

        public virtual ActionResult Student02(string fpUserId)
        {
            var model = new StudentSurveyPost5ViewModel { FpUserId = fpUserId };
            if (string.IsNullOrWhiteSpace(fpUserId) || _context.FpSurveyResponses.Any(x => x.FpUserId == fpUserId && x.SurveyId == model.QuestionId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(true));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student02(StudentSurveyPost5ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.GetSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted());
        }

        public virtual ActionResult Student03(string fpUserId)
        {
            var model = new StudentSurveyPost10ViewModel { FpUserId = fpUserId };
            if (string.IsNullOrWhiteSpace(fpUserId) || _context.FpSurveyResponses.Any(x => x.FpUserId == fpUserId && x.SurveyId == model.QuestionId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(true));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student03(StudentSurveyPost10ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.CreateSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted());
        }

        public virtual ActionResult Student04(string fpUserId)
        {
            var model = new StudentSurveyPost2WeeksViewModel { FpUserId = fpUserId };
            if (string.IsNullOrWhiteSpace(fpUserId) || _context.FpSurveyResponses.Any(x => x.FpUserId == fpUserId && x.SurveyId == model.QuestionId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(true));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student04(StudentSurveyPost2WeeksViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.CreateSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted());
        }
    }
}