using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads;
using MyJobLeads.Controllers;
using MyJobLeads.Models.Accounts;
using Moq;
using System.Web.Security;
using MyJobLeads.Infrastructure;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Tests.Mocks;

namespace MyJobLeads.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : InMemoryTestBase
    {
        [TestMethod]
        public void Index_Shows_When_Not_Logged_In()
        {
            // Setup
            HomeController controller = new HomeController(_unitOfWork);
            controller.MembershipService = new MockMembershipService(null);
            SetupController(controller);

            // Act
            ActionResult result = controller.Index();

            // Verify
            Assert.IsNotNull(result, "Index returned a null action result");
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Index did not return a view result");
        }

        [TestMethod]
        public void Index_Redirects_To_Add_Jobsearch_Action_When_User_Has_No_Job_Searches()
        {
            // Setup
            User user = new User { Id = 20, JobSearches = new List<JobSearch>() };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            HomeController controller = new HomeController(_unitOfWork);
            controller.MembershipService = new MockMembershipService(user);
            SetupController(controller);

            // Act
            ActionResult result = controller.Index();

            // Verify
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), "Index returned the incorrect type of action result");
            RedirectToRouteResult redirect = (RedirectToRouteResult)result;
            Assert.AreEqual("JobSearch", redirect.RouteValues["controller"], "Index redirected to an incorrect controller");
            Assert.AreEqual("Add", redirect.RouteValues["action"], "Index redirected to an incorrect action");
        }

        [TestMethod]
        public void Index_Redirects_To_Jobsearch_Index_When_User_Has_Job_Searches_But_Null_LastVisitedJobSearchId()
        {
            // Setup
            User user = new User { Id = 40, JobSearches = new List<JobSearch>() };
            user.JobSearches.Add(new JobSearch());
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            HomeController controller = new HomeController(_unitOfWork);
            controller.MembershipService = new MockMembershipService(user);
            SetupController(controller);

            // Act
            ActionResult result = controller.Index();

            // Verify
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), "Index returned the incorrect type of action result");
            RedirectToRouteResult redirect = (RedirectToRouteResult)result;
            Assert.AreEqual("JobSearch", redirect.RouteValues["controller"], "Index redirected to an incorrect controller");
            Assert.AreEqual("Index", redirect.RouteValues["action"], "Index redirected to an incorrect action");
        }

        [TestMethod]
        public void Index_Redirects_To_Jobsearch_View_When_User_Has_Job_Searches_And_LastVisitedJobSearchId()
        {
            // Setup
            User user = new User { Id = 20, JobSearches = new List<JobSearch>(), LastVisitedJobSearchId = 4 };
            user.JobSearches.Add(new JobSearch());
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            HomeController controller = new HomeController(_unitOfWork);
            controller.MembershipService = new MockMembershipService(user);
            SetupController(controller);

            // Act
            ActionResult result = controller.Index();

            // Verify
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), "Index returned the incorrect type of action result");
            RedirectToRouteResult redirect = (RedirectToRouteResult)result;
            Assert.AreEqual("JobSearch", redirect.RouteValues["controller"], "Index redirected to an incorrect controller");
            Assert.AreEqual("View", redirect.RouteValues["action"], "Index redirected to an incorrect action");
            Assert.AreEqual(4, redirect.RouteValues["id"], "Index redirected with an incorrect jobsearch id value");
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(_unitOfWork);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
