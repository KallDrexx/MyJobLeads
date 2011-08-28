using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using MyJobLeads.Controllers;
using Castle.Windsor.Installer;

namespace MyJobLeads.Tests.Controllers
{
    [TestClass]
    public class PositionControllerTests
    {
        [TestMethod]
        public void Windsor_Can_Resolve_PositionController_Dependencies()
        {
            // Setup
            WindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<PositionController>());

            // Act
            container.Kernel.Resolve(typeof(PositionController));
        }
    }
}
