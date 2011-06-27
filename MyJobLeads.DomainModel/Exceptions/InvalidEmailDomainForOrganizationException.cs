using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class InvalidEmailDomainForOrganizationException : MJLException
    {
        public InvalidEmailDomainForOrganizationException(string domain)
            : base(string.Format("{0} is an invalid email address domain for this organization", domain))
        {
            EmailDomain = domain;
        }

        public string EmailDomain { get; protected set; }
    }
}
