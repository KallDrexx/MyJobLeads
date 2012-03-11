using System;
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

        public virtual ActionResult SurveyCompleted(int surveysCompleted = 0)
        {
            return View();
        }

        public virtual ActionResult Student01(string fpUserId)
        {
            var model = new StudentSurveyPost3ViewModel { FpUserId = fpUserId };
            if (HasUserFilledOutSurvey(model.QuestionId, fpUserId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(fpUserId)));

            return View(model);
        }
        
        [HttpPost]
        public virtual ActionResult Student01(StudentSurveyPost3ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.CreateSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(model.FpUserId)));
        }

        public virtual ActionResult Student02(string fpUserId)
        {
            var model = new StudentSurveyPost5ViewModel { FpUserId = fpUserId };
            if (HasUserFilledOutSurvey(model.QuestionId, fpUserId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(fpUserId)));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student02(StudentSurveyPost5ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.GetSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(model.FpUserId)));
        }

        public virtual ActionResult Student03(string fpUserId)
        {
            var model = new StudentSurveyPost10ViewModel { FpUserId = fpUserId };
            if (HasUserFilledOutSurvey(model.QuestionId, fpUserId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(fpUserId)));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student03(StudentSurveyPost10ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.CreateSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(model.FpUserId)));
        }

        public virtual ActionResult Student04(string fpUserId)
        {
            var model = new StudentSurveyPost2WeeksViewModel { FpUserId = fpUserId };
            if (HasUserFilledOutSurvey(model.QuestionId, fpUserId))
                return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(fpUserId)));

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Student04(StudentSurveyPost2WeeksViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.FpSurveyResponses.Add(model.CreateSurveyResponse);
            _context.SaveChanges();

            return RedirectToAction(MVC.FillPerfect.Feedback.SurveyCompleted(GetFpCompletedSurveyCount(model.FpUserId)));
        }

        protected int GetFpCompletedSurveyCount(string fpUserId)
        {
            string userIdWithoutSchool = fpUserId.Contains("|")
                ? fpUserId.Substring(0, fpUserId.IndexOf('|'))
                : fpUserId;

            return _context.FpSurveyResponses
                           .Where(x => x.FpUserId.Contains(userIdWithoutSchool))
                           .Count();
        }

        protected bool HasUserFilledOutSurvey(string surveyId, string fpUserId)
        {
            // Consider an empty user id as a filled out survey
            if (string.IsNullOrWhiteSpace(fpUserId))
                return true;

            string userIdWithoutSchool = fpUserId.Contains("|")
                ? fpUserId.Substring(0, fpUserId.IndexOf('|'))
                : fpUserId;

            return string.IsNullOrWhiteSpace(fpUserId) ||
                    _context.FpSurveyResponses
                            .Any(x => x.FpUserId.Contains(userIdWithoutSchool) && x.SurveyId == surveyId);
        }
    }
}
