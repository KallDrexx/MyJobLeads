using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;
using Moq;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.Organizations;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.Tests.Commands.Users
{
    [TestClass]
    public class CreateUserCommandTests : EFTestBase
    {
        private void SetupOrgByTokenMock(Organization org)
        {
            Mock<OrganizationByRegistrationTokenQuery> query = new Mock<OrganizationByRegistrationTokenQuery>(_serviceFactory.Object);
            query.Setup(x => x.Execute(It.Is<OrganizationByRegistrationTokenQueryParams>(y => y.RegistrationToken == org.RegistrationToken)))
                 .Returns(org);
            _serviceFactory.Setup(x => x.GetService<OrganizationByRegistrationTokenQuery>()).Returns(query.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            Mock<UserByEmailQuery> userQuery = new Mock<UserByEmailQuery>(_unitOfWork);
            userQuery.Setup(x => x.Execute()).Returns((User)null);
            _serviceFactory.Setup(x => x.GetService<UserByEmailQuery>()).Returns(userQuery.Object);
        }

        [TestMethod]
        public void Can_Create_New_User()
        {
            // Setup

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Execute_Returns_Created_User()
        {
            // Setup

            // Act
            User user = new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            Assert.IsNotNull(user, "Execute returned a null user");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
            Assert.IsTrue(PasswordUtils.CheckPasswordHash(user.Email, "password", user.Password), "User's password could not be validated");
        }

        [TestMethod]
        public void Email_Is_Trimmed_And_Converted_To_Lower_Case()
        {
            // Setup

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = " TEST@email.com ",
                PlainTextPassword = "password"
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNotNull(user, "No user was created");
            Assert.AreEqual("test@email.com", user.Email, "User's email was incorrect");
        }

        [TestMethod]
        public void Execute_Throws_MJLDuplicateEmailException_When_Email_Already_Exists()
        {
            // Setup
            User user = new User { Email = "test@test.com" };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Commit();

            Mock<UserByEmailQuery> userQuery = new Mock<UserByEmailQuery>(_unitOfWork);
            userQuery.Setup(x => x.Execute()).Returns(user);
            _serviceFactory.Setup(x => x.GetService<UserByEmailQuery>()).Returns(userQuery.Object);

            // Act
            try
            {
                new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
                {
                    Email = "test@test.com",
                    PlainTextPassword = "pass"
                });

                Assert.Fail("Command did not throw an exception");
            }

            // Catch
            catch (MJLDuplicateEmailException ex)
            {
                Assert.AreEqual("test@test.com", ex.Email, "MJLDuplicateUsernameException's email value was incorrect");
            }
        }

        [TestMethod]
        public void Execute_Initializes_JobSearch_List()
        {
            // Act
            User result = new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            Assert.IsNotNull(result.JobSearches, "User's job search list was not initialized");
        }

        [TestMethod]
        public void User_Not_Associated_With_Organization_When_No_Registration_Token_Provided()
        {
            // Setup
            Organization org = new Organization { RegistrationToken = Guid.NewGuid() };
            _unitOfWork.Organizations.Add(org);
            _unitOfWork.Commit();

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@email.com",
                PlainTextPassword = "password"
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().SingleOrDefault();
            Assert.IsNull(user.Organization, "User was incorrectly associated with an organization");
        }

        [TestMethod]
        public void User_Can_Be_Associated_With_An_Organization()
        {
            // Setup 
            Organization org = new Organization { RegistrationToken = Guid.NewGuid() };
            _unitOfWork.Organizations.Add(org);
            _unitOfWork.Commit();

            SetupOrgByTokenMock(org);

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test",
                PlainTextPassword = "pass",
                RegistrationToken = org.RegistrationToken
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().Single();
            Organization result = user.Organization;
            Assert.AreEqual(org, result, "User was associated with an incorrect organization");
        }

        [TestMethod]
        public void Command_Throws_InvalidOrganizationRegistrationTokenException_When_Registration_Token_Isnt_Found()
        {
            // Setup 
            Guid badToken = Guid.NewGuid();
            Organization org = new Organization { RegistrationToken = Guid.NewGuid() };
            _unitOfWork.Organizations.Add(org);
            _unitOfWork.Commit();

            Mock<OrganizationByRegistrationTokenQuery> query = new Mock<OrganizationByRegistrationTokenQuery>(_serviceFactory.Object);
            query.Setup(x => x.Execute(It.Is<OrganizationByRegistrationTokenQueryParams>(y => y.RegistrationToken == org.RegistrationToken)))
                 .Returns(org);
            _serviceFactory.Setup(x => x.GetService<OrganizationByRegistrationTokenQuery>()).Returns(query.Object);

            // Act
            try
            {
                new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
                {
                    Email = "test",
                    PlainTextPassword = "pass",
                    RegistrationToken = badToken
                });
                Assert.Fail("Command did not throw an exception");
            }

            // Verify
            catch (InvalidOrganizationRegistrationTokenException ex)
            {
                Assert.AreEqual(badToken, ex.RegistrationToken, "Exception's registration token was incorrect");
            }
        }

        [TestMethod]
        public void Command_Throws_InvalidEmailDomainForOrganizationException_When_Email_Not_Allowed_For_Org()
        {
            // Setup
            Organization org = new Organization
            {
                IsEmailDomainRestricted = true,
                Name = "Test",
                RegistrationToken = Guid.NewGuid(),
                EmailDomains = new List<OrganizationEmailDomain>()
            };
            org.EmailDomains.Add(new OrganizationEmailDomain { Domain = "test.com", IsActive = true, });

            _unitOfWork.Organizations.Add(org);
            _unitOfWork.Commit();

            SetupOrgByTokenMock(org);

            // Act
            try
            {
                new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
                {
                    Email = "test@testa.com",
                    PlainTextPassword = "pass",
                    RegistrationToken = org.RegistrationToken
                });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (InvalidEmailDomainForOrganizationException ex)
            {
                Assert.AreEqual("testa.com", ex.EmailDomain, "Exception's EmailDomain value was incorrect");
            }
        }

        [TestMethod]
        public void Can_Create_User_For_Organization_When_Email_Domain_Is_Acceptable()
        {
            // Setup
            Organization org = new Organization
            {
                IsEmailDomainRestricted = true,
                Name = "Test",
                RegistrationToken = Guid.NewGuid(),
                EmailDomains = new List<OrganizationEmailDomain>()
            };
            org.EmailDomains.Add(new OrganizationEmailDomain { Domain = "test.com", IsActive = true, });

            _unitOfWork.Organizations.Add(org);
            _unitOfWork.Commit();

            SetupOrgByTokenMock(org);

            // Act
            new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
            {
                Email = "test@test.com",
                PlainTextPassword = "pass",
                RegistrationToken = org.RegistrationToken
            });

            // Verify
            User user = _unitOfWork.Users.Fetch().Single();
            Assert.AreEqual(org, user.Organization, "User was associated with an incorrect organization");
        }

        [TestMethod]
        public void Command_Throws_InvalidEmailDomainForOrganizationException_When_Email_Domain_Not_Active_For_Org()
        {
            // Setup
            Organization org = new Organization
            {
                IsEmailDomainRestricted = true,
                Name = "Test",
                RegistrationToken = Guid.NewGuid(),
                EmailDomains = new List<OrganizationEmailDomain>()
            };
            org.EmailDomains.Add(new OrganizationEmailDomain { Domain = "test.com", IsActive = false, });

            _unitOfWork.Organizations.Add(org);
            _unitOfWork.Commit();

            SetupOrgByTokenMock(org);

            // Act
            try
            {
                new CreateUserCommand(_serviceFactory.Object).Execute(new CreateUserCommandParams
                {
                    Email = "test@test.com",
                    PlainTextPassword = "pass",
                    RegistrationToken = org.RegistrationToken
                });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (InvalidEmailDomainForOrganizationException ex)
            {
                Assert.AreEqual("test.com", ex.EmailDomain, "Exception's EmailDomain value was incorrect");
            }
        }
    }
}
