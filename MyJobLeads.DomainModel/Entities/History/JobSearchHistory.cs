using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class JobSearchHistory : EntityHistoryBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual int JobSearchId { get; set; }
        public virtual JobSearch JobSearch { get; set; }
    }
}
