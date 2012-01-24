using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities.Surveys;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.Areas.FillPerfect.Models.Surveys
{
    public class StudentSurveyPost10ViewModel
    {
        public string FpUserId { get; set; }
        public string QuestionId { get { return "Student10Apps"; } }
        public string Question
        {
            get { return "You have now completed at least 10 job applications with InterviewTools Fillperfect.  Please take a minute and provide career services with feedback on the usefulness of this product."; }
        }

        [MinLength(20, ErrorMessage = "You must provide feedback with at least 20 characters")]
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
                            Answer = Comments,
                            Order = 0
                        }
                    }
                };
            }
        }
    }
}