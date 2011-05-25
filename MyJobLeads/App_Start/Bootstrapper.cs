using System;
using System.Web;
using System.Web.Mvc;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.App_Start;
using MyJobLeads.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Bootstrapper), "Wire")]

[assembly: WebActivator.ApplicationShutdownMethod(typeof(Bootstrapper), "DeWire")]

namespace MyJobLeads.App_Start
{
    public static class Bootstrapper
    {
        private static readonly IWindsorContainer container = new WindsorContainer();

        public static void Wire()
        {
            //To be able to inject IEnumerable<T> ICollection<T> IList<T> T[] use this:
            //container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            //Documentation http://docs.castleproject.org/Windsor.Resolvers.ashx
            
            //To support typed factories add this:
            //container.AddFacility<TypedFactoryFacility>();
            //Documentation http://docs.castleproject.org/Windsor.Typed-Factory-Facility.ashx
            
            container.Install(FromAssembly.This());
			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public static void DeWire()
        {
            container.Dispose();
        }
    }
}