using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasApplied { get; set; }
        public string Notes { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
