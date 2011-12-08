using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.PositionSearching;
using Moq;
using MyJobLeads.DomainModel.Entities;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.Tests.Processes.PositionSearching.LinkedIn
{
    [TestClass]
    public class LinkedInPositionDetailsTests : EFTestBase
    {
        protected IProcess<LinkedInPositionDetailsParams, ExternalPositionDetailsViewModel> _process;
        protected Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>> _verifyTokenMock;
        protected User _user;

        [TestInitialize]
        public void Init()
        {
            _verifyTokenMock = new Mock<IProcess<VerifyUserLinkedInAccessTokenParams,UserAccessTokenResultViewModel>>();
            _verifyTokenMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = true });

            _process = new LinkedInPositionSearchProcesses(_context, _verifyTokenMock.Object);

            // Initialize user with test (but valid) access token data
            _user = new User();
            var oauth = new OAuthData
            {
                Token = "bfcf3fe4-b4d4-4f37-9d32-292ae9d45347",
                Secret = "f66673b2-5877-4fbf-80e0-3826ca9f7eed",
                TokenProvider = TokenProvider.LinkedIn,
                TokenType = TokenType.AccessToken,
                LinkedInUser = _user
            };

            _user.LinkedInOAuthData = oauth;
            _context.Users.Add(_user);
            _context.OAuthData.Add(oauth);
            _context.SaveChanges();
        }

        [TestMethod]
        public void Can_Retrieve_Job_Details_From_Linked_In()
        {
            // Setup
            string positionId = "217456";
            string expTitle = "Entry Level .Net Developer - Software Engineer: C#, VB.Net, SQL";
            string expLocation = "St Petersburg ,FL";
            string expCompanyName = "CyberCoders";
            string expCompanyId = "21836";
            DateTime expPostDate = new DateTime(2011, 11, 11);
            DateTime expExpirationDate = new DateTime();
            string expExperience = "Internship";
            string expJobType = "Full-time";
            string expJobFunctions = "Information Technology";
            string expIndustries = "Computer Software";

            #region Expected Description

            string expDescription = @"Entry Level .Net Developer - Software Engineer: C#, VB.Net, SQL Entry Level .Net Developer - Software Engineer: C#, VB.Net, SQL - Skills Required - Software Engineer, Software Developer, .Net Developer, Junior Software Engineer, SWE, Junior Developer, Bachelors of Computer Science, Entry-Level, Software Engineer

Entry Level .Net Developer - Software Engineer: C#, VB.Net, SQL

Attention Recent College Graduates and Junior Developers!

My client is a mid sized Computer Software Company in the greater Tampa/Clearwater area that is currently seeking Junior and Entry Level .Net Developers to join their growing team before 2012!

My client has 15+ years experience provides state-of-the-art products and integration services to the document and forms processing industries. Their commitment to quality and customer service has made them a leader in document imaging and automated data capture industry.

If you are a recent College Graduate with your Bachelors in Computer Science and have .Net 4.0 and C# experience and are interested in joining a dynamic international team of developers that will be designing and developing several custom web and windows applications for Healthcare and Government agencies, please apply today!

What's in it for you:

- Work with cutting edge technology
- Fun attractive work environment
- Competitive Salary
- Full Benefits

CyberCoders, Inc is proud to be an Equal Opportunity Employer. Applicants are considered for all positions without regard to race, color, religion, sex, national origin, age, disability, sexual orientation, ancestry, marital or veteran status.";

            #endregion

            // Act
            var result = _process.Execute(new LinkedInPositionDetailsParams { RequestingUserId = _user.Id, PositionId = positionId });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(positionId, result.Id, "Position's id value was incorrect");
            Assert.AreEqual(expTitle, result.Title, "Position's title was incorrect");
            Assert.AreEqual(expLocation, result.Location, "Position's location was incorrect");
            Assert.AreEqual(expCompanyName, result.CompanyName, "Position's company name was incorrect");
            Assert.AreEqual(expCompanyId, result.CompanyId, "Position's company id was incorrect");
            Assert.AreEqual(expPostDate, result.PostedDate, "Position's posted date was incorrect");
            Assert.AreEqual(expExpirationDate, result.ExpirationDate, "Position's expiration date was incorrect");
            Assert.AreEqual(expExperience, result.ExperienceLevel, "Position's experience level was incorrect");
            Assert.AreEqual(expJobType, result.JobType, "Position's job type was incorrect");
            Assert.AreEqual(expJobFunctions, result.JobFunctions, "Position's job functions was incorrect");
            Assert.AreEqual(expIndustries, result.Industries, "Position's industries was incorrect");
            Assert.AreEqual(expDescription, result.Description, "Position's description was incorrect");
            Assert.AreEqual(ExternalDataSource.LinkedIn, result.DataSource, "Position's data source was incorrect");
        }
    }
}
