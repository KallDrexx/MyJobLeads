using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Data
{
    public interface IProcess<TProcessParams, TViewModel>
    {
        TViewModel Execute(TProcessParams procParams);
    }
}
