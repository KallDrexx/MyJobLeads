using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.Controllers;
using System.Reflection;
using MyJobLeads.DomainModel.Data;
using System.Runtime.CompilerServices;
using Castle.MicroKernel;
using FluentValidation;
using MyJobLeads.Infrastructure.Providers;
using MyJobLeads.DomainModel;
using MyJobLeads.DomainModel.Providers.Validation;
using Castle.MicroKernel.Registration;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Tests.WebInfrastructure
{
    [TestClass]
    public class WindsorTests
    {
        [TestMethod]
        public void Windsor_Can_Resolve_All_Command_And_Query_Classes()
        {
            // Setup
            Assembly asm = Assembly.GetAssembly(typeof(IUnitOfWork));
            IList<Type> classTypes = Assembly.GetAssembly(typeof(MJLConstants))
                              .GetTypes()
                              .Where(x => !x.IsDefined(typeof(CompilerGeneratedAttribute), false))
                              .Where(x => x.Namespace.StartsWith("MyJobLeads.DomainModel.Commands") || x.Namespace.StartsWith("MyJobLeads.DomainModel.Queries"))
                              .Where(x => x.IsClass && !x.IsDefined(typeof(CompilerGeneratedAttribute), false))
                              .Distinct()
                              .ToList();

            IWindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<HomeController>());

            string assertOutput = "The following types could not be resolved: " + Environment.NewLine;
            int failureCount = 0;

            // Act
            foreach (Type t in classTypes)
            {
                try { container.Resolve(t); }
                catch (ComponentNotFoundException)
                {
                    assertOutput += t.FullName + Environment.NewLine;
                    failureCount++;
                }
            }

            // Verify
            Assert.IsTrue(failureCount == 0, assertOutput + string.Format("{0} classes missing from Windsor", failureCount));
        }

        [TestMethod]
        public void Can_Resolve_Validator_Factory()
        {
            // Setup
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<HomeController>());

            // Act
            IValidatorFactory factory = container.Resolve<IValidatorFactory>();

            // verify
            Assert.AreEqual(typeof(WindsorValidatorFactory), factory.GetType(), "Unexpected validator factory found");
        }

        [TestMethod]
        public void Can_Resolve_All_Validators()
        {
            // Setup
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<HomeController>());

            Assembly asm = Assembly.GetAssembly(typeof(IUnitOfWork));
            IList<Type> validators = asm.GetTypes().Where(x => x.GetInterfaces().Any(y => y.IsDerivedFromOpenGenericType(typeof(IValidator<>)))).ToList();

            string assertOutput = "The following types could not be resolved: " + Environment.NewLine;
            int failureCount = 0;

            // Act
            foreach (Type t in validators)
            {
                // Get the IValidator interface and try to resolve the interface
                var typeToResolve = t.GetInterfaces().Where(x => x.IsDerivedFromOpenGenericType(typeof(IValidator<>)))
                                                     .Where(x => x.IsGenericType)
                                                     .Single();

                try
                {
                    object resolvedType = container.Resolve(typeToResolve);
                    if (resolvedType.GetType() != t)
                    {
                        assertOutput += string.Format("{0} was not resolved for the {1} interface", t.FullName, typeToResolve.FullName);
                        failureCount++;
                    }
                }
                catch (ComponentNotFoundException)
                {
                    assertOutput += typeToResolve.FullName + Environment.NewLine;
                    failureCount++;
                }
            }

            // Verify
            Assert.AreEqual(0, failureCount, assertOutput);
        }
    }
}
