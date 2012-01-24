using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities.Surveys;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.Areas.FillPerfect.Models.Surveys
{
    public class StudentSurveyPost3ViewModel
    {
        public string FpUserId { get; set; }
        public string QuestionId { get { return "Student3Apps"; } }

        [Range(1, 5, ErrorMessage="You must select a valid response on the scale")]
        public int Answer { get; set; }
        public string Question
        {
            get { return "You have now completed at least 3 job applications with InterviewTools Fillperfect.  Please take a minute and rate, on a scale from 1 to 5, your initial impression of the tool."; }
        }

        public string Comments { get; set; }

        public FpSurveyResponse CreateSurveyResponse
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