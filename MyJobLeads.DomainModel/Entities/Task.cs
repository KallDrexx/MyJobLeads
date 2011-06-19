using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.History;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Category { get; set; }

        [StringLength(int.MaxValue)]
        public string Notes { get; set; }

        public virtual Company Company { get; set; }
        public virtual int? CompanyId { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual int? ContactId { get; set; }

        public virtual ICollection<TaskHistory> History { get; set; }
    }
}
