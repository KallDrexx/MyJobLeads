using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities.Surveys;

namespace MyJobLeads.Areas.FillPerfect.Models.Surveys
{
    public class StudentSurvey01ViewModel
    {
        public string FpUserId { get; set; }
        public string QuestionId { get { return "Student01"; } }
        public int Answer { get; set; }
        public string Question
        {
            get { return "On a scale from 1-10, how would you rate your overall impression of the tool?"; }
        }

        public FpSurveyResponse GetSurveyResponse
        {
            get
            {
                return new FpSurveyResponse
                {
                    Date = DateTime.Now,
                    FpUserId = FpUserId,
                    SurveyId = QuestionId,
                    Answers = new List<FpSurveyResponseAnswers>
                    {
                        new FpSurveyResponseAnswers
                        {
                            Question = Question,
                            Answer = Answer.ToString(),
                            Order = 0
                        }
                    }
                };
            }
        }
    }
}