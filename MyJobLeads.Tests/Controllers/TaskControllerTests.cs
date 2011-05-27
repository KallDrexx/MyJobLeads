using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.Controllers;

namespace MyJobLeads.Tests.Controllers
{
    [TestClass]
    public class TaskControllerTests
    {
        [TestMethod]
        public void Windsor_Can_Resolve_TaskController_Dependencies()
        {
            // Setup
            WindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<TaskController>());

            // Act
            container.Kernel.Resolve(typeof(TaskController));
        }
    }
}
