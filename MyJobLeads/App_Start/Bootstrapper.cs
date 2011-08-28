using System;
using System.Web;
using System.Web.Mvc;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.App_Start;
using MyJobLeads.Infrastructure;
using System.Configuration;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity;
using MyJobLeads.DomainModel.EntityMapping;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Bootstrapper), "Wire")]

[assembly: WebActivator.ApplicationShutdownMethod(typeof(Bootstrapper), "DeWire")]

namespace MyJobLeads.App_Start
{
    public static class Bootstrapper
    {
        private static IWindsorContainer container = new WindsorContainer();
        private const string UpdateDbAppSettingName = "UpdateDatabaseOnModelChange";

        public static void Wire()
        {
            // Set the Entity Framework database initializer
            if (ConfigurationManager.AppSettings[UpdateDbAppSettingName] == "true")
                Database.SetInitializer<MyJobLeadsDbContext>(new MyJobLeadsDbInitializer());
            else
                Database.SetInitializer<MyJobLeadsDbContext>(null);
            
            container.Install(FromAssembly.This());
			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // Load all entity mapping configurations
            EntityMapLoader.LoadEntityMappings();
        }

        public static void DeWire()
        {
            container.Dispose();
        }

        public static IWindsorContainer WindsorContainer
        {
            get { return container; }
        }
    }
}