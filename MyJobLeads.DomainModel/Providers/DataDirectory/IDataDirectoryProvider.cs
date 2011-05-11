using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Providers.DataDirectory
{
    public interface IDataDirectoryProvider
    {
        string DataDirectoryPath { get; }
    }
}
