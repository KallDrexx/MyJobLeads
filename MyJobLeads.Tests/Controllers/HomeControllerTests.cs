using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads;
using MyJobLeads.Controllers;
using MyJobLeads.ViewModels.Accounts;
using Moq;
using System.Web.Security;
using MyJobLeads.Infrastructure;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Tests.Mocks;
using MvcContrib.TestHelper;
using MyJobLeads.DomainModel.Queries.Users;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace MyJobLeads.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : EFTestBase
    {

        [TestMethod]
        public void Windsor_Can_Resolve_HomeController_Dependencies()
        {
            // Setup
            WindsorContainer container = new WindsorContainer();
            container.Kernel.ComponentModelBuilder.AddContributor(new SingletonLifestyleEqualizer());
            container.Install(FromAssembly.Containing<HomeController>());

            // Act
            container.Kernel.Resolve(typeof(HomeController));
        }

        [TestMethod]
        public void Index_Shows_When_Not_Logged_In()
        {
            // Setup
            var userByIdMock = new Mock<UserByIdQuery>(null);
            userByIdMock.Setup(x => x.Execute()).Returns((User)null);
            HomeController controller = new HomeController(userByIdMock.Object, _serviceFactory.Object);

            // Act
            ActionResult result = controller.Index();

            // Verify
            result.AssertViewRendered();
        }

        [TestMethod]
        public void Index_Redirects_To_Add_Jobsearch_Action_When_User_Has_No_Job_Searches()
        {
            // Setup
            User user = new User { Id = 20, JobSearches = new List<JobSearch>() };
            var userByIdMock = new Mock<UserByIdQuery>(null);
            userByIdMock.Setup(x => x.Execute()).Returns(user);
            HomeController controller = new HomeController(userByIdMock.Object, _serviceFactory.Object);
            controller.CurrentUserId = 20;

            // Act
            ActionResult result = controller.Index();

            // Verify
            result.AssertActionRedirect().ToController("JobSearch").ToAction("Add");
        }

        [TestMethod]
        public void Index_Redirects_To_Jobsearch_Index_When_User_Has_Job_Searches_But_Null_LastVisitedJobSearchId()
        {
            // Setup
            User user = new User { Id = 40, JobSearches = new List<JobSearch>() };
            user.JobSearches.Add(new JobSearch());
            var userByIdMock = new Mock<UserByIdQuery>(null);
            userByIdMock.Setup(x => x.Execute()).Returns(user);

            HomeController controller = new HomeController(userByIdMock.Object, _serviceFactory.Object);
            controller.CurrentUserId = 40;

            // Act
            ActionResult result = controller.Index();

            // Verify
            result.AssertActionRedirect().ToController("JobSearch").ToAction("Index");
        }

        [TestMethod]
        public void Index_Redirects_To_Jobsearch_View_When_User_Has_Job_Searches_And_LastVisitedJobSearchId()
        {
            // Setup
            User user = new User { Id = 20, JobSearches = new List<JobSearch>(), LastVisitedJobSearchId = 4 };
            var userByIdMock = new Mock<UserByIdQuery>(null);
            userByIdMock.Setup(x => x.Execute()).Returns(user);

            HomeController controller = new HomeController(userByIdMock.Object, _serviceFactory.Object);
            controller.CurrentUserId = 20;

            // Act
            ActionResult result = controller.Index();

            // Verify
            result.AssertActionRedirect().ToController("JobSearch").ToAction("Details").WithParameter("id", 4);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(null, _serviceFactory.Object);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            result.AssertViewRendered();
        }
    }
}
