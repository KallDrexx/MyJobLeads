using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Utilities;
using System.Security.Cryptography;

namespace MyJobLeads.Tests.Utilities
{
    [TestClass]
    public class EncryptionUtilsTests
    {
        [TestMethod]
        public void Can_Encrypt_And_Decrypt_String()
        {
            // Setup
            var util = new EncryptionUtils();
            string origString = "My String abcd123";
            RijndaelManaged myRijndael = new RijndaelManaged();

            // Act
            var encryptedValue = util.EncryptStringToBytes_AES(origString, myRijndael.Key, myRijndael.IV);
            string result = util.DecryptStringFromBytes_AES(encryptedValue, myRijndael.Key, myRijndael.IV);

            // Verify
            Assert.AreEqual(origString, result);
        }
    }
}
