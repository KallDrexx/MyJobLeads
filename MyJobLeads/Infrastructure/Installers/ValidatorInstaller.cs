using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Reflection;
using FluentValidation;
using MyJobLeads.DomainModel;

namespace MyJobLeads.Infrastructure.Installers
{
    public class ValidatorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Retrieve a list of all implemented validators and extract the IValidator<T> generic for it,
            //  Implement the validator from the IValidator<T> interface
            IList<Type> validators = Assembly.GetAssembly(typeof(TypeExtensions))
                                             .GetTypes()
                                             .Where(x => x.GetInterfaces().Any(y => y.IsDerivedFromOpenGenericType(typeof(IValidator<>))))
                                             .ToList();

            foreach (Type t in validators)
            {
                // Get the IValidator<T> interface
                Type valInterface = t.GetInterfaces().Where(x => x.IsDerivedFromOpenGenericType(typeof(IValidator<>)))
                                                     .Where(x => x.IsGenericType)
                                                     .Single();

                // Register the validator
                container.Register(Component.For(valInterface).ImplementedBy(t).LifeStyle.Transient);
            }
        }
    }
}