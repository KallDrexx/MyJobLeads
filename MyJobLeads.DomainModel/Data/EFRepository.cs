using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity;

namespace MyJobLeads.DomainModel.Data
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public EFRepository(MyJobLeadsDbContext context)
        {
            _objectSet = context.Set<T>();
        }

        public IQueryable<T> Fetch()
        {
            return _objectSet;
        }

        public void Add(T newEntity)
        {
            _objectSet.Add(newEntity);
        }

        public void Remove(T entity)
        {
            _objectSet.Remove(entity);
        }

        protected DbSet<T> _objectSet;
    }
}
