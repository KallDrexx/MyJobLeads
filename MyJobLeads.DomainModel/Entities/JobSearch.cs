﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities
{
    public class JobSearch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
