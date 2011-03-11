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
        private IRepository<UnitTestEntity> _unitTestEntities;
        public IRepository<UnitTestEntity> UnitTestEntities
        {
            get
            {
                if (_unitTestEntities == null)
                    _unitTestEntities = new InMemoryRepository<UnitTestEntity>(_list);
                return _unitTestEntities;
            }
        }

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
