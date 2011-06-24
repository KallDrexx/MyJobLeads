using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid RegistrationToken { get; set; }

        public virtual ICollection<User> Members { get; set; }
    }
}
