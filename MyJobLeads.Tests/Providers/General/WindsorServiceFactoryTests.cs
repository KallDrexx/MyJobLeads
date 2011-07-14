using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.Infrastructure.Providers;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace MyJobLeads.Tests.Providers
{
    public class TestInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MockServiceTestClass>().ImplementedBy<MockServiceTestClass>().LifeStyle.Singleton);
        }
    }

    public class MockServiceTestClass
    {

    }

    [TestClass]
    public class WindsorServiceFactoryTests
    {
        [TestMethod]
        public void MJLServiceNotFoundException_Thrown_When_Provider_Doesnt_Have_Service_Instance()
        {
            // Setup
            IServiceFactory provider = new WindsorServiceFactory(new WindsorContainer());

            // Act
            try
            {
                provider.GetService<WindsorServiceFactory>();
                Assert.Fail("No exception occurred but one was expected");
            }

            // Verify
            catch (MJLServiceNotFoundException ex)
            {
                Assert.AreEqual(typeof(WindsorServiceFactory), ex.RequestedServiceType, "MJLServiceNotFoundException's requested service type was incorrect");
            }
        }

        [TestMethod]
        public void Service_Provider_Retrieves_Requested_Service_Class()
        {
            // Setup
            IWindsorContainer container = new WindsorContainer();
            container.Install(new TestInstaller());
            WindsorServiceFactory provider = new WindsorServiceFactory(container);

            // Act
            MockServiceTestClass result = provider.GetService<MockServiceTestClass>();

            // Verify
            Assert.IsNotNull(result, "Service provider returned a null service object");
        }
    }
}
