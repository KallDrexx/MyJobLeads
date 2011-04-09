using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System;
using System.Collections.Generic;
using MyJobLeads.Controllers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Tests.Mocks;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MyJobLeads.DomainModel.Queries.JobSearches;
using Moq;

namespace MyJobLeads.Tests.Controllers
{
    [TestClass]
    public class JobSearchControllerTest : InMemoryTestBase
    {
        [TestMethod]
        public void Windsor_Can_Resolve_HomeController_Dependencies()
        {
            // Setup
            WindsorContainer container = new WindsorContainer();
            container.Install(FromAssembly.Containing<JobSearchController>());

            // Act
            container.Kernel.Resolve(typeof(JobSearchController));
        }

        [TestMethod]
        public void Index_Shows_All_Job_Searches_For_User()
        {
            // Setup
            JobSearch search1 = new JobSearch { Id = 3 }, search2 = new JobSearch { Id = 4 };
            var mock = new Mock<JobSearchesByUserIdQuery>(null);
            mock.Setup(x => x.Execute()).Returns(new List<JobSearch> { search1, search2 });

            var controller = new JobSearchController(mock.Object, null, null, null, null, null);
            var builder = new TestControllerBuilder();
            builder.InitializeController(controller);

            // Act
            ActionResult result = controller.Index();

            // Verify
            result.AssertViewRendered().ForView("");
            IList<JobSearch> model = ((ViewResult)result).Model as List<JobSearch>;

            Assert.IsNotNull(model, "Action returned a null view model");
            Assert.AreEqual(2, model.Count, "Returned view model had an incorrect number of elements");
            Assert.AreEqual(search1.Id, model[0].Id, "First job search had an incorrect id value");
            Assert.AreEqual(search2.Id, model[1].Id, "Second job search had an incorrect id value");
        }

        [TestMethod]
        public void Add_Returns_Edit_View()
        {
            // Setup
            var controller = new JobSearchController(null, null, null, null, null, null);

            // Act
            ActionResult result = controller.Add();

            // Verify
            result.AssertViewRendered().ForView(MVC.JobSearch.Views.Edit);
        }
    }
}
