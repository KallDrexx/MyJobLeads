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
using MvcContrib.TestHelper;

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
            result.AssertViewRendered();
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
            result.AssertActionRedirect().ToController("JobSearch").ToAction("Add");
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
            result.AssertActionRedirect().ToController("JobSearch").ToAction("Index");
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
            result.AssertActionRedirect().ToController("JobSearch").ToAction("View").WithParameter("id", 4);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(_unitOfWork);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            result.AssertViewRendered();
        }
    }
}
