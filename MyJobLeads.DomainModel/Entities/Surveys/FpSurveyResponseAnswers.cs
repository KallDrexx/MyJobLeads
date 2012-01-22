using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Surveys
{
    public class FpSurveyResponseAnswers
    {
        public int Id { get; set; }
        public short Order { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public virtual FpSurveyResponse Survey { get; set; }
        public int SurveyId { get; set; }
    }
}
