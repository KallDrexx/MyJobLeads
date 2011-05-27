using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MyJobLeads.DomainModel.Providers;
using Castle.Windsor;
using Castle.MicroKernel;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Infrastructure.Providers
{
    public class WindsorServiceFactory : IServiceFactory
    {
        public WindsorServiceFactory(IWindsorContainer windsorContainer)
        {
            _container = windsorContainer;
        }

        public ServiceType GetService<ServiceType>() where ServiceType : class
        {
            // Use windsor to resolve the service class.  If the dependency can't be resolved throw an exception
            try { return _container.Resolve<ServiceType>(); }
            catch (ComponentNotFoundException) { throw new MJLServiceNotFoundException(typeof(ServiceType)); }
        }

        #region Member Variables

        protected IWindsorContainer _container;

        #endregion
    }
}
