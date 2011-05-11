using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers.DataDirectory;

namespace MyJobLeads.Infrastructure.Installers
{
    public class MyJobLeadsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<EFUnitOfWork>().LifeStyle.PerWebRequest);
            container.Register(Component.For<ISearchProvider>().ImplementedBy<LuceneSearchProvider>().LifeStyle.PerWebRequest);
            container.Register(Component.For<IDataDirectoryProvider>().ImplementedBy<AppDataDirectoryProvider>().LifeStyle.Singleton);
        }
    }
}