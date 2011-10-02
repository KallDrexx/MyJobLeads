using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using Moq;

namespace MyJobLeads.Tests.Queries.Tasks
{
    [TestClass]
    public class TaskByIdQueryTests : EFTestBase
    {
        private Task _task1, _task2, _task3;

        private void InitializeTestEntities()
        {
            _task1 = new Task { Name = "Task 1" };
            _task2 = new Task { Name = "Task 2" };
            _task3 = new Task { Name = "Task 3" };

            _unitOfWork.Tasks.Add(_task1);
            _unitOfWork.Tasks.Add(_task2);
            _unitOfWork.Tasks.Add(_task3);

            _unitOfWork.Commit();
        }

        [TestMethod]
        public void Can_Get_Task_By_ID()
        {
            // Setup
            InitializeTestEntities();
            var authMock = new Mock<IProcess<TaskAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<TaskAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            // Act
            Task result = new TaskByIdQuery(_unitOfWork, authMock.Object).WithTaskId(_task2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null task");
            Assert.AreEqual(_task2.Id, result.Id, "Returned task had an incorrect id value");
            Assert.AreEqual(_task2.Name, result.Name, "Returned task had an incorrect name value");
        }

        [TestMethod]
        public void Execute_Returns_Null_Task_When_Id_Not_Found()
        {
            // Setup
            InitializeTestEntities();
            var authMock = new Mock<IProcess<TaskAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<TaskAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });
            int id = _task2.Id + 100;

            // Act
            Task result = new TaskByIdQuery(_unitOfWork, authMock.Object).WithTaskId(id).Execute();

            // Verify
            Assert.IsNull(result, "Query returned a non-null task");
        }

        [TestMethod]
        public void Execute_Returns_Null_Task_When_Not_Authorized()
        {
            // Setup
            InitializeTestEntities();
            var authMock = new Mock<IProcess<TaskAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<TaskAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });

            // Act
            Task result = new TaskByIdQuery(_unitOfWork, authMock.Object)
                                .WithTaskId(_task2.Id)
                                .RequestedByUserId(15)
                                .Execute();

            // Verify
            Assert.IsNull(result, "Query returned a non-null task");
            authMock.Verify(x => x.Execute(It.Is<TaskAuthorizationParams>(y => y.RequestingUserId == 15)));
        }
    }
}
