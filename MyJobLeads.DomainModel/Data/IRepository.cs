using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Fetch();
        void Add(T newEntity);
        void Remove(T entity);
    }
}
