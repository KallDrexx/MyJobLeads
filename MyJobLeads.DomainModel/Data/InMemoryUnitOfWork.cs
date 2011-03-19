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
        public IRepository<Company> Companies { get { return new InMemoryRepository<Company>(_list); } }
        public IRepository<Contact> Contacts { get { return new InMemoryRepository<Contact>(_list); } }
        public IRepository<JobSearch> JobSearches { get { return new InMemoryRepository<JobSearch>(_list); } }
        public IRepository<Task> Tasks { get { return new InMemoryRepository<Task>(_list); } }
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
