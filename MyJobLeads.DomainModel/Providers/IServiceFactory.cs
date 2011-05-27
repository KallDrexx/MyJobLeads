using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Providers
{
    public interface IServiceFactory
    {
        /// <summary>
        /// Retrieves an instantiated service class of the specified type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        ServiceType GetService<ServiceType>() where ServiceType : class;
    }
}
