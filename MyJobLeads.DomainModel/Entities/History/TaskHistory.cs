using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class TaskHistory : EntityHistoryBase
    {
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public bool Completed { get; set; }

        public virtual int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
