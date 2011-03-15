using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLEntityNotFoundException : MJLException
    {
        public MJLEntityNotFoundException(Type entityType, string idValue)
            : base(string.Concat("No entity of type '", entityType.ToString(), "' exists with an idvalue of '", idValue, "'"))
        {
            EntityType = entityType;
            IdValue = idValue;
        }

        public MJLEntityNotFoundException(Type entityType, int idValue)
            : base(string.Concat("No entity of type '", entityType.ToString(), "' exists with an idvalue of '", idValue, "'"))
        {
            EntityType = entityType;
            IdValue = Convert.ToString(idValue);
        }

        public Type EntityType { get; protected set; }
        public string IdValue { get; protected set; }
    }
}
