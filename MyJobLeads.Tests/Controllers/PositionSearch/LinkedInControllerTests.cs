using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.Areas.PositionSearch.Controllers;
using Castle.Windsor.Installer;
using Castle.Windsor;

namespace MyJobLeads.Tests.Controllers.PositionSearch
{
    [TestClass]
    public class LinkedInControllerTests
    {
        [TestMethod]
        public void Windsor_Can_Resolve_HomeController_Dependencies()
        {
            // Setup
            WindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<LinkedInController>());

            // Act
            container.Kernel.Resolve(typeof(LinkedInController));
        }
    }
}
