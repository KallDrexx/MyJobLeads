using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.Metrics;
using MyJobLeads.DomainModel.Processes.Organizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using Moq;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.Organizations
{
    [TestClass]
    public class OrganizationMemberStatisticsTests : EFTestBase
    {
        [TestMethod]
        public void Can_Get_Members_With_Statistics_For_Organization()
        {
            // Setup
            var org = new Organization();
            var user1 = new User
            {
                Organization = org,
                FullName = "name 1",
                Email = "email 1",
                LastVisitedJobSearch = new JobSearch
                {
                    Metrics = new JobSearchMetrics
                    {
                        NumApplyTasksCompleted = 1,
                        NumApplyTasksCreated = 2,
                        NumCompaniesCreated = 3,
                        NumContactsCreated = 4,
                        NumInPersonInterviewTasksCreated = 5,
                        NumPhoneInterviewTasksCreated = 6
                    }
                }
            };

            var user2 = new User
            {
                Organization = org,
                FullName = "name 2",
                Email = "email 2",
                LastVisitedJobSearch = new JobSearch
                {
                    Metrics = new JobSearchMetrics
                    {
                        NumApplyTasksCompleted = 7,
                        NumApplyTasksCreated = 8,
                        NumCompaniesCreated = 9,
                        NumContactsCreated = 10,
                        NumInPersonInterviewTasksCreated = 11,
                        NumPhoneInterviewTasksCreated = 12
                    }
                }
            };

            _context.Users.Add(user1);
            _context.Users.Add(user2);
            _context.SaveChanges();

            var authMock = new Mock<IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<OrganizationAdminAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel> process = new OrganizationMetricQueryProcesses(_context, authMock.Object);

            // Act
            var result = process.Execute(new OrganizationMemberStatisticsParams { OrganizationId = org.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsNotNull(result.MemberStats, "Resulting member list was null");
            Assert.AreEqual(2, result.MemberStats.Count, "Member list had an incorrect number of elements");
            Assert.IsTrue(result.MemberStats.Any(x => x.Id == user1.Id), "Member list did not contain the first user");
            Assert.IsTrue(result.MemberStats.Any(x => x.Id == user2.Id), "Member list did not contain the second user");
            Assert.AreEqual(org.Id, result.OrganizationId, "Organization Id was incorrect");

            var firstUser = result.MemberStats.Where(x => x.Id == user1.Id).Single();
            var metrics1 = user1.LastVisitedJobSearch.Metrics;
            Assert.AreEqual(user1.FullName, firstUser.FullName, "First user's full name was incorrect");
            Assert.AreEqual(user1.Email, firstUser.Email, "First user's email was incorrect");
            Assert.AreEqual(metrics1.NumApplyTasksCompleted, firstUser.Metrics.NumApplyTasksCompleted, "First user's number of apply tasks completed was incorrect");
            Assert.AreEqual(metrics1.NumApplyTasksCreated, firstUser.Metrics.NumApplyTasksCreated, "First user's number of apply tasks created was incorrect");
            Assert.AreEqual(metrics1.NumCompaniesCreated, firstUser.Metrics.NumCompaniesCreated, "First user's number of companies was incorrect");
            Assert.AreEqual(metrics1.NumContactsCreated, firstUser.Metrics.NumContactsCreated, "First user's number of contacts was incorrect");
            Assert.AreEqual(metrics1.NumInPersonInterviewTasksCreated, firstUser.Metrics.NumInPersonInterviewTasksCreated, "First user's number in person interview tasks was incorrect");
            Assert.AreEqual(metrics1.NumPhoneInterviewTasksCreated, firstUser.Metrics.NumPhoneInterviewTasksCreated, "First user's number of phone interview tasks was incorrect");

            var secondUser = result.MemberStats.Where(x => x.Id == user2.Id).Single();
            var metrics2 = user2.LastVisitedJobSearch.Metrics;
            Assert.AreEqual(user2.FullName, secondUser.FullName, "First user's full name was incorrect");
            Assert.AreEqual(user2.Email, secondUser.Email, "First user's email was incorrect");
            Assert.AreEqual(metrics2.NumApplyTasksCompleted, secondUser.Metrics.NumApplyTasksCompleted, "First user's number of apply tasks completed was incorrect");
            Assert.AreEqual(metrics2.NumApplyTasksCreated, secondUser.Metrics.NumApplyTasksCreated, "First user's number of apply tasks created was incorrect");
            Assert.AreEqual(metrics2.NumCompaniesCreated, secondUser.Metrics.NumCompaniesCreated, "First user's number of companies was incorrect");
            Assert.AreEqual(metrics2.NumContactsCreated, secondUser.Metrics.NumContactsCreated, "First user's number of contacts was incorrect");
            Assert.AreEqual(metrics2.NumInPersonInterviewTasksCreated, secondUser.Metrics.NumInPersonInterviewTasksCreated, "First user's number in person interview tasks was incorrect");
            Assert.AreEqual(metrics2.NumPhoneInterviewTasksCreated, secondUser.Metrics.NumPhoneInterviewTasksCreated, "First user's number of phone interview tasks was incorrect");
        }

        [TestMethod]
        public void Does_Not_Retrieve_Members_Of_Different_Organization()
        {
            // Setup 
            var org1 = new Organization();
            var org2 = new Organization();
            var user = new User { Organization = org2 };

            _context.Users.Add(user);
            _context.Organizations.Add(org1);
            _context.SaveChanges();

            var authMock = new Mock<IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<OrganizationAdminAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });

            IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel> process = new OrganizationMetricQueryProcesses(_context, authMock.Object);

            // Act
            var result = process.Execute(new OrganizationMemberStatisticsParams { OrganizationId = org1.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsNotNull(result.MemberStats, "Process retured a null member list");
            Assert.AreEqual(0, result.MemberStats.Count, "Member list had an incorrect number of elements");
        }

        [TestMethod]
        public void Throws_UserNotAuthorizedForEntityException_If_Not_Organization_Admin()
        {
            // Setup
            var org = new Organization();
            var user = new User { Organization = org };
            _context.Users.Add(user);
            _context.SaveChanges();

            var authMock = new Mock<IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<OrganizationAdminAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = false });
            IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel> process = new OrganizationMetricQueryProcesses(_context, authMock.Object);

            // Act
            try
            {
                process.Execute(new OrganizationMemberStatisticsParams { OrganizationId = org.Id, RequestingUserId = user.Id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (UserNotAuthorizedForEntityException ex)
            {
                Assert.AreEqual(typeof(Organization), ex.EntityType, "Exception's entity type was incorrect");
                Assert.AreEqual(org.Id, ex.IdValue, "Exception's id value was incorrect");
                Assert.AreEqual(user.Id, ex.UserId, "Exception's user id value was incorrect");
            }
        }

        [TestMethod]
        public void Calls_Auth_Process_With_Correct_OrgId_and_UserId()
        {
            // Setup
            var authMock = new Mock<IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel>>();
            authMock.Setup(x => x.Execute(It.IsAny<OrganizationAdminAuthorizationParams>())).Returns(new AuthorizationResultViewModel { UserAuthorized = true });
            IProcess<OrganizationMemberStatisticsParams, OrganizationMemberStatisticsViewModel> process = new OrganizationMetricQueryProcesses(_context, authMock.Object);

            // Act
            process.Execute(new OrganizationMemberStatisticsParams { OrganizationId = 12, RequestingUserId = 13 });

            // Verify
            authMock.Verify(x => x.Execute(It.Is<OrganizationAdminAuthorizationParams>(y => y.UserId == 13 && y.OrganizationId == 12)), Times.Once());
        }
    }
}
