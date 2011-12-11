﻿using System;
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
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.Commands.Companies;

namespace MyJobLeads.Tests.Processes.PositionSearching.LinkedIn
{
    [TestClass]
    public class LinkedInPositionDetailsTests : EFTestBase
    {
        protected IProcess<LinkedInPositionDetailsParams, ExternalPositionDetailsViewModel> _process;
        protected Mock<IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>> _verifyTokenMock;
        protected User _user;
        protected Mock<IProcess<CreatePositionParams, PositionDisplayViewModel>> _createPositionMock;

        [TestInitialize]
        public void Init()
        {
            _verifyTokenMock = new Mock<IProcess<VerifyUserLinkedInAccessTokenParams,UserAccessTokenResultViewModel>>();
            _verifyTokenMock.Setup(x => x.Execute(It.IsAny<VerifyUserLinkedInAccessTokenParams>())).Returns(new UserAccessTokenResultViewModel { AccessTokenValid = true });

            _createPositionMock = new Mock<IProcess<CreatePositionParams, PositionDisplayViewModel>>();
            var createCompanyCmd = new CreateCompanyCommand(_serviceFactory.Object);

            _process = new LinkedInPositionSearchProcesses(_context, _verifyTokenMock.Object, createCompanyCmd, _createPositionMock.Object, null);

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
            string expTitle = "Research Manager supporting Forbes.com worldwide online ad sales team";
            string expLocation = "Greater New York City Area";
            string expCompanyName = "Forbes.com Inc.";
            string expCompanyId = "21836";
            DateTime expPostDate = new DateTime(2007, 1, 14);
            string expExperience = "Mid-Senior level";
            string expJobType = "Full-time";
            string expJobFunctions = "Research";
            string expIndustries = "Online Media";
            string expDescription = @"Forbes.com Research Manager<br><br>Based in New York, the research manager is responsible for managing all research services and projects, with an opportunity to be part of an expanding research department. This position serves as a strategic partner with Forbes.com’s worldwide advertising sales team.  This position plays a central role in maintaining Forbes.com’s position as a pioneer in original research for the online business leadership market.<br><br>Primary Job Responsibilities:<br><br>Lead and present strategic primary research efforts to support sales. <br><br>Manage the department and extensive requests generated by sales including ad effectiveness research.<br><br>Manage budgets and relationships with third party research vendors (e.g., CMR, MMX, NNR, Dynamic Logic, InsightExpress).  <br><br>Set strategies and best practices for media and ad effectiveness research.<br><br>Represent Forbes.com on major industry committees (e.g., IAB, ARF, OPA)<br> <br>Skills and Qualifications:<br><br>·         Strong understanding of both online and offline media research<br><br>·         Excellent presentation skills <br><br>·         Strong market research and survey research experience<br><br>·         Strong strategic and analytical skills as well as detail oriented<br><br>·         Desire to work in a fast-paced, fun and demanding environment.<br><br>·         High energy level and great interpersonal skills.";

            // Act
            var result = _process.Execute(new LinkedInPositionDetailsParams { RequestingUserId = _user.Id, PositionId = positionId });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(positionId, result.Id, "Position's id value was incorrect");
            Assert.AreEqual(expTitle, result.Title, "Position's title was incorrect");
            Assert.AreEqual(expLocation, result.Location, "Position's location was incorrect");
            Assert.AreEqual(expCompanyName, result.CompanyName, "Position's company name was incorrect");
            //Assert.AreEqual(expCompanyId, result.CompanyId, "Position's company id was incorrect");
            Assert.AreEqual(expPostDate, result.PostedDate, "Position's posted date was incorrect");
            Assert.AreEqual(expExperience, result.ExperienceLevel, "Position's experience level was incorrect");
            Assert.AreEqual(expJobType, result.JobType, "Position's job type was incorrect");
            Assert.AreEqual(expJobFunctions, result.JobFunctions, "Position's job functions was incorrect");
            Assert.AreEqual(expIndustries, result.Industries, "Position's industries was incorrect");
            Assert.AreEqual(expDescription, result.Description, "Position's description was incorrect");
            Assert.IsFalse(result.IsActive, "Position was incorrect set as active");

            Assert.AreEqual("CNaU-cXF34", result.JobPosterId, "Job poster id was incorrect");
            Assert.AreEqual("Bruce R.", result.JobPosterName, "Job poster name was incorrect");
            Assert.AreEqual("Chief Insights Officer at Forbes", result.JobPosterHeadline, "Job poster's headline was incorrect");

            Assert.AreEqual(ExternalDataSource.LinkedIn, result.DataSource, "Position's data source was incorrect");
        }

        [TestMethod]
        public void Returns_Null_When_Invalid_Id()
        {
            // Setup
            string positionId = "217456999999999";

            // Act
            var result = _process.Execute(new LinkedInPositionDetailsParams { RequestingUserId = _user.Id, PositionId = positionId });

            // Verify
            Assert.IsNull(result, "Process did not return a null result");
        }
    }
}
