using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.PositionSearching;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Enums;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Processes.OAuth;

namespace MyJobLeads.Tests.Processes.PositionSearching.LinkedIn
{
    [TestClass]
    public class VerifyLinkedInAccessTokenTests : EFTestBase
    {
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _process;
        protected User _user;

        [TestInitialize]
        public void Initialize()
        {
            _process = new LinkedInOAuthProcesses(_context);

            _user = new User();
            _context.Users.Add(_user);
            _context.SaveChanges();
        }

        [TestMethod]
        public void Token_Not_Valid_When_User_Has_No_LinkedIn_Token()
        {
            // Act
            var result = _process.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = _user.Id });
            
            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.AccessTokenValid, "Process incorrectly specified the access token as valid");
        }

        [TestMethod]
        public void Token_Not_Valid_When_User_Doesnt_Exist()
        {
            // Act
            var result = _process.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = _user.Id + 100 });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.AccessTokenValid, "Process incorrectly specified the access token as valid");
        }

        [TestMethod]
        public void Token_Not_Valid_When_LinkedIn_Access_Token_Is_Invalid()
        {
            // Setup
            var oauth = new OAuthData
            {
                Token = Guid.NewGuid().ToString(),
                Secret = "f66673b2-5877-4fbf-80e0-3826ca9f7eed",
                TokenProvider = TokenProvider.LinkedIn,
                TokenType = TokenType.AccessToken
            };

            _context.OAuthData.Add(oauth);
            _user.LinkedInOAuthData = oauth;
            _context.SaveChanges();

            // Act
            var result = _process.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = _user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsFalse(result.AccessTokenValid, "Process incorrectly specified the access token as valid");
        }

        [TestMethod]
        public void Token_Is_Valid_After_LinkedIn_Validation()
        {
            // Setup
            var oauth = new OAuthData
            {
                Token = "bfcf3fe4-b4d4-4f37-9d32-292ae9d45347",
                Secret = "f66673b2-5877-4fbf-80e0-3826ca9f7eed",
                TokenProvider = TokenProvider.LinkedIn,
                TokenType = TokenType.AccessToken
            };

            _context.OAuthData.Add(oauth);
            _user.LinkedInOAuthData = oauth;
            _context.SaveChanges();

            // Act
            var result = _process.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = _user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.AccessTokenValid, "Process incorrectly specified the access token as not valid");
        }
    }
}
