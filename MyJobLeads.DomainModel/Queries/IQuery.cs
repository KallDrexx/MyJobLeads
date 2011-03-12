using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Queries
{
    public interface IQuery<T> where T : class
    {
        T Execute();
    }
}
