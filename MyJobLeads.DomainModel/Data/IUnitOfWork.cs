using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Data
{
    public interface IUnitOfWork
    {
        // Repositories
        IRepository<UnitTestEntity> UnitTestEntities { get; }
        IRepository<User> Users { get; }

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
