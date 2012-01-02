using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.ContactSearching
{
    public class ExternalContactAddedResultViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
    }
}
