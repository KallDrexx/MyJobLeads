using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class PositionHistory : EntityHistoryBase
    {
        public string Title { get; set; }
        public bool HasApplied { get; set; }
        public string Notes { get; set; }
        public string LinkedInId { get; set; }

        public int? PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
