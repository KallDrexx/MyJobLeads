using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities.Surveys;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.Areas.FillPerfect.Models.Surveys
{
    public class StudentSurveyPost2WeeksViewModel
    {
        public string FpUserId { get; set; }
        public string QuestionId { get { return "Student2Weeks"; } }

        [Range(1, 10, ErrorMessage="You must select a response on the rating scale")]
        public int Answer { get; set; }
        public string Question
        {
            get { return "You have been using InterviewTools FillPerfect for over two weeks now.  Please take a minute and and provide career services with some feedback of the usefulness of this product."; }
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