using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class OAuthResponseNotAvailableException : MJLException
    {
        public OAuthResponseNotAvailableException(string message) : base(message) { }
    }
}
