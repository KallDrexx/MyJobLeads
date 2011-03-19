using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public bool Completed { get; set; }

        public virtual Company Company { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
