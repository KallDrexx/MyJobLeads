using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;

namespace MyJobLeads.DomainModel.Entities.EF
{
    public class MyJobLeadsDbContext : DbContext
    {
        // Entities
        public DbSet<UnitTestEntity> UnitTestEntities { get; set; }
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Contains the Entity Framework database configuratoin rules
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
