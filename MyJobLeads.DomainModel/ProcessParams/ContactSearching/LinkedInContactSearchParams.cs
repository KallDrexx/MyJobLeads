using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.ContactSearching
{
    public class LinkedInContactSearchParams
    {
        public int RequestingUserId { get; set; }
        public string Keywords { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public bool RestrictToCurrentCompany { get; set; }
        public string Title { get; set; }
        public bool RestrictToCurrentTitle { get; set; }
        public string SchoolName { get; set; }
        public bool RestrictToCurrentSchool { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }

        public int RequestedPageNumber { get; set; }
    }
}
