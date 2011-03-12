using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Data
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        protected IList<object> _list;

        public InMemoryUnitOfWork()
        {
            _list = new List<object>();
        }

        // Repositories
        public IRepository<UnitTestEntity> UnitTestEntities { get { return new InMemoryRepository<UnitTestEntity>(_list); } }
        public IRepository<User> Users { get { return new InMemoryRepository<User>(_list); } }

        // UOW Methods
        public void Commit()
        {
            // No-op
        }

        public void BeginTransaction()
        {
            // No-op
        }

        public void EndTransaction(bool commit)
        {
            // No-op
        }
    }
}
