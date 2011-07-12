﻿using System;
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
            IList<Type> classTypes = asm.GetTypes()
                                        .Where(x => x.Namespace.StartsWith("MyJobLeads.DomainModel.Commands") || x.Namespace.StartsWith("MyJobLeads.DomainModel.Queries"))
                                        .Where(x => x.IsClass)
                                        .Where(x => x.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0)
                                        .OrderBy(x => x.FullName)
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
    }
}
