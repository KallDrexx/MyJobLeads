using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Positions;
using Moq;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.Queries.Positions;

namespace MyJobLeads.Tests.Processes.Positions
{
    [TestClass]
    public class GetPositionListTests : EFTestBase
    {
        [TestMethod]
        public void Can_Get_List_Of_Positions_For_Users_Current_JobSearch()
        {
            // Setup
            var posAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            posAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            JobSearch js1 = new JobSearch(), js2 = new JobSearch();
            User user = new User { LastVisitedJobSearch = js1 };
            Company comp1 = new Company { JobSearch = js1 }, comp2 = new Company { JobSearch = js2 };
            Position position1 = new Position { Company = comp1 }, position2 = new Position { Company = comp2 }, position3 = new Position { Company = comp1 };

            _context.Positions.Add(position1);
            _context.Positions.Add(position2);
            _context.Positions.Add(position3);
            _context.Users.Add(user);
            _context.SaveChanges();

            IProcess<GetPositionListForUserParams, PositionListViewModel> process = new PositionProcesses(_context, posAuthMock.Object, companyAuthMock.Object);

            // Act
            PositionListViewModel result = process.Execute(new GetPositionListForUserParams { UserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null view model");
            Assert.AreEqual(2, result.Positions.Count, "Incorrect number of positions returned");
            Assert.IsTrue(result.Positions.Any(x => x.Id == position1.Id), "No position in returned list had an id for position #1");
            Assert.IsTrue(result.Positions.Any(x => x.Id == position3.Id), "No position in returned list had an id for position #3");
        }
    }
}
