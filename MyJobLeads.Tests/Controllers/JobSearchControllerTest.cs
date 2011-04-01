using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System;
using System.Collections.Generic;
using MyJobLeads.Controllers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Tests.Mocks;
using System.Web.Mvc;

namespace MyJobLeads.Tests.Controllers
{
    [TestClass]
    public class JobSearchControllerTest : InMemoryTestBase
    {
        [TestMethod]
        public void Index_Shows_All_Job_Searches_For_User()
        {
            // Setup
            User user = new User { Id = 20, JobSearches = new List<JobSearch>() };
            JobSearch search1 = new JobSearch { Id = 3, User = user }, search2 = new JobSearch { Id = 4, User = user };

            _unitOfWork.Users.Add(user);
            _unitOfWork.JobSearches.Add(search1);
            _unitOfWork.JobSearches.Add(search2);
            _unitOfWork.Commit();

            JobSearchController controller = new JobSearchController(_unitOfWork);
            controller.MembershipService = new MockMembershipService(user);

            // Act
            ActionResult result = controller.Index();

            // Verify
            Assert.IsNotNull(result, "A null ActionResult was returned");
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Returned ActionResult was of an incorrect type");

            ViewResult vresult = result as ViewResult;
            Assert.IsInstanceOfType((result as ViewResult).Model, typeof(List<JobSearch>), "Returned view model was incorrect");

            List<JobSearch> model = vresult.Model as List<JobSearch>;
            Assert.AreEqual(2, model.Count, "Returned view model had an incorrect number of elements");
            Assert.AreEqual(search1.Id, model[0].Id, "First job search had an incorrect id value");
            Assert.AreEqual(search2.Id, model[1].Id, "Second job search had an incorrect id value");
        }
    }
}
