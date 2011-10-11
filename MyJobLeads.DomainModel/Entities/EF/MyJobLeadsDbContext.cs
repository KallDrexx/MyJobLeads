using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.DomainModel.Entities.EF
{
    public class MyJobLeadsDbContext : DbContext
    {
        // Entities
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyHistory> CompanyHistory { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactHistory> ContactHistory { get; set; }
        public DbSet<JobSearch> JobSearches { get; set; }
        public DbSet<JobSearchHistory> JobSearchHistory { get; set; }
        public DbSet<OfficialDocument> OfficialDocuments { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistory { get; set; }
        public DbSet<UnitTestEntity> UnitTestEntities { get; set; }
        public DbSet<User> Users { get; set; }

        // Configuration Tables
        public DbSet<MilestoneConfig> MilestoneConfigs { get; set; }

        /// <summary>
        /// Contains the Entity Framework database configuration rules
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
