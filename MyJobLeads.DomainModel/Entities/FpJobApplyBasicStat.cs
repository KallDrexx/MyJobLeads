using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class FpJobApplyBasicStat
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime PostedDate { get; set; }
        public int ApplyCount { get; set; }
    }
}
