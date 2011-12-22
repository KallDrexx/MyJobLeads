using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.ContactSearching
{
    public class ExternalContactDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Headline { get; set; }
        public string ProfileUrl { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }
        public IList<ContactPositionViewModel> Positions { get; set; }

        public class ContactPositionViewModel
        {
            public string Title { get; set; }
            public string Summary { get; set; }
            public string CompanyName { get; set; }
            public string CompanyId { get; set; }
            public bool CurrentPosition { get; set; }
        }
    }
}
