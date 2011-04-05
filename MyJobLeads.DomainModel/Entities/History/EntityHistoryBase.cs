using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.History
{
    public class EntityHistoryBase
    {
        public int Id { get; set; }
        public DateTime DateModified { get; set; }
        public string HistoryAction { get; set; }

        public virtual int AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}
