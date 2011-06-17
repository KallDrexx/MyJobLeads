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
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.Infrastructure.Providers;
using FluentValidation;

namespace MyJobLeads.Infrastructure.Installers
{
    public class MyJobLeadsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Pass the windsor container to be used for IWindsorContainer dependency injection
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            // Miscellaneous components
            container.Register(Component.For<IServiceFactory>().ImplementedBy<WindsorServiceFactory>().LifeStyle.PerWebRequest);
            container.Register(Component.For<ISearchProvider>().ImplementedBy<LuceneSearchProvider>().LifeStyle.PerWebRequest);
            container.Register(Component.For<IDataDirectoryProvider>().ImplementedBy<AppDataDirectoryProvider>().LifeStyle.Singleton);
            container.Register(Component.For<IValidatorFactory>().ImplementedBy<WindsorValidatorFactory>().LifeStyle.Singleton);

            // After registering the EFUnitOfWork, we have to create a new MyJobLeadsDbContext, or else the
            //   errors will occur if the site is starting up and a 2nd request is made
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<EFUnitOfWork>().LifeStyle.PerWebRequest);
            //using (var context = new MyJobLeadsDbContext())
            //{
            //    context.Set<UnitTestEntity>().Any();
            //}
        }
    }
}