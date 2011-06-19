using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class TaskHistory : EntityHistoryBase
    {
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Category { get; set; }

        [StringLength(int.MaxValue)]
        public string Notes { get; set; }

        public virtual int TaskId { get; set; }
        public virtual Task Task { get; set; }

        public virtual int? ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
