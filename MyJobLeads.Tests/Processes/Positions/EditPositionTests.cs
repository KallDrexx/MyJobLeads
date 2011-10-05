﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Positions;
using MyJobLeads.DomainModel.Exceptions;
using Moq;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;

namespace MyJobLeads.Tests.Processes.Positions
{
    [TestClass]
    public class EditPositionTests : EFTestBase
    {
        [TestMethod]
        public void Can_Edit_Position()
        {
            // Setup
            var posAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            posAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<EditPositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, posAuthMock.Object, companyAuthMock.Object);
            Position position = new Position
            {
                HasApplied = true,
                Title = "start title",
                Notes = "start note"
            };
            _context.Positions.Add(position);
            _context.SaveChanges();

            // Act
            process.Execute(new EditPositionParams
            {
                Id = position.Id,
                Title = "title",
                HasApplied = false,
                Notes = "notes"
            });

            // Verify
            Position result = _context.Positions.Single();
            Assert.AreEqual("title", result.Title, "Position's title was incorrect");
            Assert.AreEqual("notes", result.Notes, "Position's note was incorrect");
            Assert.IsFalse(result.HasApplied, "Position's has applied value was incorrect");
        }

        [TestMethod]
        public void Editing_Position_Returns_Edited_Position_ViewModel()
        {
            // Setup
            var posAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            posAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<EditPositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, posAuthMock.Object, companyAuthMock.Object);
            Position position = new Position
            {
                HasApplied = true,
                Title = "start title",
                Notes = "start note"
            };
            _context.Positions.Add(position);
            _context.SaveChanges();

            // Act
            PositionDisplayViewModel result = process.Execute(new EditPositionParams
            {
                Id = position.Id,
                Title = "title",
                HasApplied = false,
                Notes = "notes"
            });

            // Verify
            Assert.AreEqual("title", result.Title, "Position's title was incorrect");
            Assert.AreEqual("notes", result.Notes, "Position's note was incorrect");
            Assert.IsFalse(result.HasApplied, "Position's has applied value was incorrect");
        }

        [TestMethod]
        public void Editing_Position_Process_Throws_EntityNotFoundException_When_Process_Not_Found()
        {
            // Setup
            var posAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            posAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<EditPositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, posAuthMock.Object, companyAuthMock.Object);
            Position position = new Position
            {
                HasApplied = true,
                Title = "start title",
                Notes = "start note"
            };
            _context.Positions.Add(position);
            _context.SaveChanges();
            int id = position.Id + 1;

            // Act
            try
            {
                process.Execute(new EditPositionParams { Id = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Position), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }

        [TestMethod]
        public void Process_Throws_UserNotAuthorizedForEntityException_When_Authorization_Fails()
        {
            // Setup
            var posAuthMock = new Mock<IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>>();
            posAuthMock.Setup(x => x.Execute(It.IsAny<PositionAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });

            var companyAuthMock = new Mock<IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>>();
            companyAuthMock.Setup(x => x.Execute(It.IsAny<CompanyQueryAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<EditPositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context, posAuthMock.Object, companyAuthMock.Object);
            Position position = new Position
            {
                HasApplied = true,
                Title = "start title",
                Notes = "start note"
            };
            _context.Positions.Add(position);
            _context.SaveChanges();

            // Act
            try
            {
                process.Execute(new EditPositionParams
                {
                    Id = position.Id,
                    Title = "title",
                    HasApplied = false,
                    Notes = "notes",
                    RequestingUserId = 15
                });

                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(Position), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(position.Id, ex.IdValue, "Exception's id value was incorrect");
                Assert.AreEqual(15, ex.UserId, "Exception's user id value was incorrect");
                posAuthMock.Verify(x => x.Execute(It.Is<PositionAuthorizationParams>(y => y.RequestingUserId == 15 && y.PositionId == position.Id)));
            }
        }
    }
}
