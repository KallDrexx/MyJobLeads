using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Transactions;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        protected MyJobLeadsDbContext _context;
        protected TransactionScope _transaction;

        #region Unit Of Work Methods

        public EFUnitOfWork()
        {
            _context = new MyJobLeadsDbContext();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            // Retrieve the context and explicitely open the database connectection
            ObjectContext ctx = ((IObjectContextAdapter)_context).ObjectContext;
            if (ctx.Connection.State != ConnectionState.Open)
                ctx.Connection.Open();

            // Open the transaction scope
            _transaction = new TransactionScope();
        }

        public void EndTransaction(bool commit)
        {
            if (_transaction == null)
                return;

            if (commit)
                _transaction.Complete();
            else
                _transaction.Dispose();

            _transaction = null;
        }

        #endregion

        #region Repositories
        private IRepository<UnitTestEntity> _unitTestEntities;
        public IRepository<UnitTestEntity> UnitTestEntities
        {
            get
            {
                if (_unitTestEntities == null)
                    _unitTestEntities = new EFRepository<UnitTestEntity>(_context);
                return _unitTestEntities;
            }
        }

        private IRepository<User> _users;
        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new EFRepository<User>(_context);
                return _users;
            }
        }

        #endregion
    }
}
