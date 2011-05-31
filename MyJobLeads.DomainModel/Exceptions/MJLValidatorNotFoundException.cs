using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLValidatorNotFoundException : MJLException
    {
        public MJLValidatorNotFoundException(Type serviceType, Type validatingEntityType)
            : base(string.Format("No validator was found for the {0} service to validate the {1} type", serviceType.ToString(), validatingEntityType.ToString()))
        {
            ServiceType = serviceType;
            ValidatingEntityType = validatingEntityType;
        }

        public Type ServiceType { get; set; }
        public Type ValidatingEntityType { get; set; }
    }
}
