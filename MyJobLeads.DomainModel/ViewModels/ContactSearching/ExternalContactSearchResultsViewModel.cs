using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.ContactSearching
{
    public class ExternalContactSearchResultsViewModel
    {
        public int DisplayedPageNumber { get; set; }
        public int TotalResultsCount { get; set; }
        public int PageSize { get; set; }
        public IList<ContactResultViewModel> Results { get; set; }

        public class ContactResultViewModel
        {
            public string ContactId { get; set; }
            public string CompanyId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Headline { get; set; }
            public string Company { get; set; }
            public string PublicUrl { get; set; }
            public bool HasAccess { get; set; }
            public DateTime LastUpdatedDate { get; set; }
        }
    }
}
