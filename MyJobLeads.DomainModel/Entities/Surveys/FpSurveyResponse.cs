using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Surveys
{
    public class FpSurveyResponse
    {
        public int Id { get; set; }
        public string FpUserId { get; set; }
        public string SurveyId { get; set; }
        public DateTime Date { get; set; }

        public IList<FpSurveyResponseAnswer> Answers { get; set; }

        public FpSurveyResponse()
        {
            Answers = new List<FpSurveyResponseAnswer>();
        }
    }
}
