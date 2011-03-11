using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Data
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        public InMemoryRepository(IList<object> list)
        {
            _list = list;
        }

        public IQueryable<T> Fetch()
        {
            return _list.Where(x => x is T).Select(x => (T)x).AsQueryable();
        }

        public void Add(T newEntity)
        {
            _list.Add(newEntity);
        }

        public void Remove(T entity)
        {
            _list.Remove(entity);
        }

        protected IList<object> _list;
    }
}
