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
            result.AssertViewRendered();
            IList<JobSearch> model = ((ViewResult)result).Model as List<JobSearch>;

            Assert.IsNotNull(model, "Action returned a null view model");
            Assert.AreEqual(2, model.Count, "Returned view model had an incorrect number of elements");
            Assert.AreEqual(search1.Id, model[0].Id, "First job search had an incorrect id value");
            Assert.AreEqual(search2.Id, model[1].Id, "Second job search had an incorrect id value");
        }
    }
}
