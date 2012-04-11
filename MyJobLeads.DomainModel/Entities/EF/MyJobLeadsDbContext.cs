using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Entities.Surveys;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.Entities.Admin;
using MyJobLeads.DomainModel.Entities.Ordering;
using System.Reflection;
using MyJobLeads.DomainModel.Entities.EF.Configuration;

namespace MyJobLeads.DomainModel.Entities.EF
{
    public class MyJobLeadsDbContext : DbContext
    {
        // Entities
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyHistory> CompanyHistory { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactHistory> ContactHistory { get; set; }
        public DbSet<FpJobApplyBasicStat> FpJobApplyBasicStats { get; set; }
        public DbSet<FpSurveyResponse> FpSurveyResponses { get; set; }
        public DbSet<JigsawAccountDetails> JigsawAccountDetails { get; set; }
        public DbSet<JobSearch> JobSearches { get; set; }
        public DbSet<JobSearchHistory> JobSearchHistory { get; set; }
        public DbSet<OfficialDocument> OfficialDocuments { get; set; }
        public DbSet<OAuthData> OAuthData { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<SiteReferralCode> SiteReferralCodes { get; set; }
        public DbSet<SiteReferral> SiteReferrals { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistory { get; set; }
        public DbSet<UnitTestEntity> UnitTestEntities { get; set; }
        public DbSet<User> Users { get; set; }

        // FillPerfect Entities
        public DbSet<FpOrgPilotUsedLicense> FpOrgPilotUsedLicenses { get; set; }
        public DbSet<FillPerfectContactResponse> FillPerfectContactResponses { get; set; }

        // Configuration Tables
        public DbSet<MilestoneConfig> MilestoneConfigs { get; set; }

        // Licenses
        public DbSet<FpUserLicense> FpUserLicenses { get; set; }

        // Ordering
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Contains the Entity Framework database configuration rules
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Load all entity configuration classes dynamically
            var typesToRegister = Assembly.GetAssembly(typeof(UserConfiguration))
                                          .GetTypes()
                                          .Where(type => type.Namespace != null && type.Namespace.Equals(typeof(UserConfiguration).Namespace))
                                          .Where(type => type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            // Set up the 1:1 connections that can't be automatically determined
            modelBuilder.Entity<OAuthData>().HasOptional(x => x.LinkedInUser).WithOptionalDependent(x => x.LinkedInOAuthData);
            modelBuilder.Entity<User>().HasOptional(x => x.JigsawAccountDetails).WithRequired(x => x.AssociatedUser);
            modelBuilder.Entity<JobSearch>().HasMany(x => x.LastVisitedUsers).WithOptional(x => x.LastVisitedJobSearch);
        }
    }
}
