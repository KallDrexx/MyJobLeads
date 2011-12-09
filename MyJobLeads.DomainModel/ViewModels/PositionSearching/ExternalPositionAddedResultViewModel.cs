using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.PositionSearching
{
    public class ExternalPositionAddedResultViewModel
    {
        public int CompanyId { get; set; }
        public int PositionId { get; set; }
        public bool AddedSuccessfully { get; set; }
    }
}
