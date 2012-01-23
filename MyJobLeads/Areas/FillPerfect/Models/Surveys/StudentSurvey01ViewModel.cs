using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities.Surveys;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.Areas.FillPerfect.Models.Surveys
{
    public class StudentSurvey01ViewModel
    {
        public string FpUserId { get; set; }
        public string QuestionId { get { return "Student01"; } }

        [Range(1, 10, ErrorMessage="You must select a valid response")]
        public int Answer { get; set; }
        public string Question
        {
            get { return "On a scale from 1-10, how would you rate your overall impression of the tool?"; }
        }

        public string Comments { get; set; }

        public FpSurveyResponse GetSurveyResponse
        {
            get
            {
                return new FpSurveyResponse
                {
                    Date = DateTime.Now,
                    FpUserId = FpUserId,
                    SurveyId = QuestionId,
                    Answers = new List<FpSurveyResponseAnswer>
                    {
                        new FpSurveyResponseAnswer
                        {
                            Question = Question,
                            Answer = Answer.ToString(),
                            Order = 0
                        },

                        new FpSurveyResponseAnswer
                        {
                            Question = "Comments",
                            Answer = Comments,
                            Order = 1
                        }
                    }
                };
            }
        }
    }
}