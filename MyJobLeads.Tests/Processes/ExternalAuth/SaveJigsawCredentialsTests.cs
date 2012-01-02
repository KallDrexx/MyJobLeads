using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuth.Jigsaw;
using MyJobLeads.DomainModel.ViewModels;
using Moq;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Processes.ExternalAuth;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.ExternalAuth
{
    [TestClass]
    public class SaveJigsawCredentialsTests : EFTestBase
    {
        IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel> _process;

        [TestMethod]
        public void Can_Save_Jigsaw_Encrypted_Credentials_For_User()
        {
            // Setup
            string username = "username";
            string unencryptedPass = "base password";
            byte[] expectedKey = Convert.FromBase64String("LHRqdBadRUVh3cdMYoeuYVng/qQlgvBReEAg5JvO48E=");
            byte[] expectedIV = Convert.FromBase64String("CB7hOrmWxs2riRF26Imn7w==");

            var encryptionMock = new Mock<EncryptionUtils>();
            encryptionMock.Setup(x => x.EncryptStringToBytes_AES(unencryptedPass, expectedKey, expectedIV)).Returns(new byte[] { Convert.ToByte(true) });

            var user = new User();
            _context.Users.Add(user);
            _context.SaveChanges();

            _process = new JigsawAuthProcesses(_context, encryptionMock.Object);

            // Act
            var result = _process.Execute(new SaveJigsawUserCredentialsParams
            {
                RequestingUserId = user.Id,
                JigsawUsername = username,
                JigsawPassword = unencryptedPass
            });

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
            Assert.IsTrue(result.WasSuccessful, "Process' result was not successful");

            var jigsaw = _context.JigsawAccountDetails.SingleOrDefault();
            Assert.IsNotNull(jigsaw, "No jigsaw credentials entity was created");
            Assert.AreEqual(username, jigsaw.Username, "Jigsaw username was incorrect");
            Assert.AreEqual(Convert.ToBase64String(new byte[] { Convert.ToByte(true) }), jigsaw.EncryptedPassword, "Jigsaw password was incorrect");
        }

        [TestMethod]
        public void Jigsaw_Credentials_Gets_Updated_If_One_Exists_For_User()
        {
            // Setup
            var encryptionMock = new Mock<EncryptionUtils>();
            encryptionMock.Setup(x => x.EncryptStringToBytes_AES(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(new byte[] { Convert.ToByte(true) });

            var user = new User { JigsawAccountDetails = new JigsawAccountDetails { Username = "a", EncryptedPassword = "b" } };
            _context.Users.Add(user);
            _context.SaveChanges();

            _process = new JigsawAuthProcesses(_context, encryptionMock.Object);

            // Act
            var result = _process.Execute(new SaveJigsawUserCredentialsParams
            {
                RequestingUserId = user.Id,
                JigsawUsername = "a",
                JigsawPassword = "b"
            });

            // Verify
            Assert.AreEqual(1, _context.JigsawAccountDetails.Count(), "Incorrect number of jigsaw records exist");
            Assert.AreEqual("a", user.JigsawAccountDetails.Username, "Jigsaw username was incorrect");
            Assert.AreEqual(Convert.ToBase64String(new byte[] { Convert.ToByte(true) }), user.JigsawAccountDetails.EncryptedPassword, "Jigsaw password was incorrect");
        }

        [TestMethod]
        public void Throws_EntityNotFoundException_When_User_Not_Found()
        {
            // Setup
            var encryptionMock = new Mock<EncryptionUtils>();
            encryptionMock.Setup(x => x.EncryptStringToBytes_AES(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(new byte[] { Convert.ToByte(true) });

            var user = new User { JigsawAccountDetails = new JigsawAccountDetails { Username = "a", EncryptedPassword = "b" } };
            _context.Users.Add(user);
            _context.SaveChanges();

            int id = user.Id + 101;
            _process = new JigsawAuthProcesses(_context, encryptionMock.Object);

            // Act
            try
            {
                _process.Execute(new SaveJigsawUserCredentialsParams { RequestingUserId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(User), ex.EntityType, "Exception's entity type value was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
            }
        }
    }
}
