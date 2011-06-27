using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.DomainModel.Data
{
    public interface IUnitOfWork
    {
        // Repositories
        IRepository<Company> Companies { get; }
        IRepository<CompanyHistory> CompanyHistory { get; }
        IRepository<Contact> Contacts { get; }
        IRepository<ContactHistory> ContactHistory { get; }
        IRepository<JobSearch> JobSearches { get; }
        IRepository<JobSearchHistory> JobSearchHistory { get; }
        IRepository<Organization> Organizations { get; }
        IRepository<Task> Tasks { get; }
        IRepository<TaskHistory> TaskHistory { get; }
        IRepository<UnitTestEntity> UnitTestEntities { get; }
        IRepository<User> Users { get; }

        // Configuration Repositories
        IRepository<MilestoneConfig> MilestoneConfigs { get; }

        /// <summary>
        /// Commits all changes to the database
        /// </summary>
        void Commit();

        /// <summary>
        /// Starts a new transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Ends the current transaction
        /// </summary>
        /// <param name="commit">True if the transaction should be committed, or false if the transaction should be rolled back</param>
        void EndTransaction(bool commit);
    }
}
