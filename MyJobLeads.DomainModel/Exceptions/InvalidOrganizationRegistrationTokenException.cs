using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class InvalidOrganizationRegistrationTokenException : MJLException
    {
        public InvalidOrganizationRegistrationTokenException(Guid token)
            : base(string.Format("The registration token '{0}' is invalid", token))
        {
            RegistrationToken = token;
        }

        public Guid RegistrationToken { get; protected set; }
    }
}
