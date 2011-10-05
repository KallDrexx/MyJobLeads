using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class UserNotAuthorizedForEntityException : Exception
    {
        public UserNotAuthorizedForEntityException(Type entityType, int idValue, int userId)
            : base(string.Format("User {0} is not authorized to access the {1} with an id of {2}", userId, entityType.Name, idValue))
        {
            EntityType = entityType;
            IdValue = idValue;
            UserId = userId;
        }

        public Type EntityType { get; set; }
        public int IdValue { get; set; }
        public int UserId { get; set; }
    }
}
