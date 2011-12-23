using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using Moq;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuthorization.Jigsaw;
using MyJobLeads.DomainModel.ViewModels.ExternalAuth;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.ExternalAuth;

namespace MyJobLeads.Tests.Processes.ExternalAuth
{
    [TestClass]
    public class GetUserJigsawCredentialTests : EFTestBase
    {
        protected IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel> _process;

        [TestMethod]
        public void Can_Get_Decrypted_Credentials()
        {
            // Setup
            string decryptedString = "decrypted string";
            string username = "username";
            string encryptedString = "w6AnE3eWg1urf54jkTejCku+JT3gPT78e9MAkUa8fsI=";
            byte[] expectedKey = Convert.FromBase64String("LHRqdBadRUVh3cdMYoeuYVng/qQlgvBReEAg5JvO48E=");
            byte[] expectedIV = Convert.FromBase64String("CB7hOrmWxs2riRF26Imn7w==");
            byte[] expectedEncryptedPass = Convert.FromBase64String(encryptedString);

            var encryptionMock = new Mock<EncryptionUtils>();
            encryptionMock.Setup(x => x.DecryptStringFromBytes_AES(expectedEncryptedPass, expectedKey, expectedIV)).Returns(decryptedString);

            var user = new User();
            var jigsaw = new JigsawAccountDetails { AssociatedUser = user, Username = username, EncryptedPassword = encryptedString };
            _context.JigsawAccountDetails.Add(jigsaw);
            _context.SaveChanges();

            _process = new JigsawAuthProcesses(_context, encryptionMock.Object);

            // Act
            var result = _process.Execute(new GetUserJigsawCredentialsParams { RequestingUserId = user.Id });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.AreEqual(username, result.JigsawUsername, "An incorrect username was returned");
            Assert.AreEqual(decryptedString, result.JigsawPassword, "An incorrect password was returned");
        }

        [TestMethod]
        public void Returns_Null_If_No_Credentials_Exist()
        {
            // Setup
            var encryptionMock = new Mock<EncryptionUtils>();
            _process = new JigsawAuthProcesses(_context, encryptionMock.Object);

            // Act
            var result = _process.Execute(new GetUserJigsawCredentialsParams { RequestingUserId = 1 });

            // Verify
            Assert.IsNull(result);
        }
    }
}