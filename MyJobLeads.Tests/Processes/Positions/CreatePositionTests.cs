using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Positions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using Moq;
using MyJobLeads.DomainModel.Queries.Positions;

namespace MyJobLeads.Tests.Processes.Positions
{
    [TestClass]
    public class CreatePositionTests : EFTestBase
    {
        [TestMethod]
        public void Can_Create_Position()
        {
            // Setup
            var positionAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            positionAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, positionAuthMock.Object, companyAuthMock.Object);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();

            // Act
            process.Execute(new CreatePositionParams
            {
                CompanyId = company.Id,
                Title = "title",
                HasApplied = true,
                Notes = "Notes",
                LinkedInId = "ABC123"
            });

            // Verify
            Position result = _context.Positions.Single();
            Assert.AreEqual("title", result.Title, "Position had an incorrect title");
            Assert.IsTrue(result.HasApplied, "Position had an incorrect HasApplied value");
            Assert.AreEqual("Notes", result.Notes, "Position had an incorrect note");
            Assert.AreEqual("ABC123", result.LinkedInId, "Position had an incorrect linked in id value");
        }

        [TestMethod]
        public void Position_Creation_Returns_Display_View_Model()
        {
            // Setup
            var positionAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            positionAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, positionAuthMock.Object, companyAuthMock.Object);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();

            // Act
            PositionDisplayViewModel result = process.Execute(new CreatePositionParams
            {
                CompanyId = company.Id,
                Title = "title",
                HasApplied = true,
                Notes = "Notes",
                LinkedInId = "ABC123"
            });

            // Verify
            Assert.AreEqual("title", result.Title, "Position had an incorrect title");
            Assert.IsTrue(result.HasApplied, "Position had an incorrect HasApplied value");
            Assert.AreEqual("Notes", result.Notes, "Position had an incorrect note");
            Assert.AreEqual("ABC123", result.LinkedInId, "Position had an incorrect LinkedIn id");
        }

        [TestMethod]
        public void Process_Throws_EntityNotFoundException_When_Company_Not_Found()
        {
            // Setup
            var positionAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            positionAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, positionAuthMock.Object, companyAuthMock.Object);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();
            int id = company.Id + 1;

            // Act
            try
            {
                process.Execute(new CreatePositionParams { CompanyId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Company), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Process_Throws_UserNotAuthorizedForEntityException_When_User_Not_Authorized_For_Company()
        {
            // Setup
            var positionAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            positionAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });

            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, positionAuthMock.Object, companyAuthMock.Object);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();

            // Act
            try
            {
                process.Execute(new CreatePositionParams
                {
                    CompanyId = company.Id,
                    Title = "title",
                    HasApplied = true,
                    Notes = "Notes",
                    RequestingUserId = 15
                });

                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(Company), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(company.Id, ex.IdValue, "Exception's id value was incorrect");
                Assert.AreEqual(15, ex.UserId, "Exception's user id value was incorrect");
                companyAuthMock.Verify(x => x.Execute(It.Is<CompanyQueryAuthorizationParams>(y => y.RequestingUserId == 15 && y.CompanyId == company.Id)));
            }
        }
    }
}