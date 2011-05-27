using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLServiceNotFoundException : MJLException
    {
        public MJLServiceNotFoundException(Type requestedServiceType)
            : base(string.Format("No service class of type {0} was found", requestedServiceType.ToString()))
        {
            RequestedServiceType = requestedServiceType;
        }

        public Type RequestedServiceType { get; set; }
    }
}
