using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Providers.DataDirectory;

namespace MyJobLeads.Infrastructure
{
    public class AppDataDirectoryProvider : IDataDirectoryProvider
    {
        public string DataDirectoryPath
        {
            get 
            { 
                // Currently mapping to Arvixe's data directory in the home directory
                return (AppDomain.CurrentDomain.GetData("DataDirectory") as string) + "..\\..\\..\\data"; 
            }
        }
    }
}