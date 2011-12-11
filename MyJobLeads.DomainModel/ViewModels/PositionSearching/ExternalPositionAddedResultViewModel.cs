using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.PositionSearching
{
    public class ExternalPositionAddedResultViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int PositionId { get; set; }
        public string PositionTitle { get; set; }
    }
}
