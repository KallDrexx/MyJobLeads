using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netDumbster.smtp;
using MyJobLeads.DomainModel.Utilities;
using System.Net.Mail;

namespace MyJobLeads.Tests.Utilities
{
    [TestClass]
    public class EmailUtilsTest
    {
        private SimpleSmtpServer _server;

        [TestInitialize]
        public void Setup()
        {
            // Create an SMTP client to get the port specified in app.config
            SmtpClient client = new SmtpClient();
            _server = SimpleSmtpServer.Start(client.Port);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _server.Stop();
        }

        [TestMethod]
        public void Can_Send_Email()
        {
            // Setup
            string toAddress = "to@address.com", fromAddress = "from@address.com", subject = "subject", message = "This is the message";

            // Act
            EmailUtils.Send(toAddress, subject, message);

            // Verify
            Assert.AreEqual(1, _server.ReceivedEmailCount, "The smtp server had an incorrect email count");
            Assert.AreEqual(1, _server.ReceivedEmail[0].ToAddresses.Count(), "The sent email had an incorrect number of to addresses");
            Assert.AreEqual(toAddress, _server.ReceivedEmail[0].ToAddresses[0].Address, "The sent email had an incorrect to address");
            Assert.AreEqual(fromAddress, _server.ReceivedEmail[0].FromAddress.Address, "The sent email had an incorrect from address");
            Assert.AreEqual(subject, _server.ReceivedEmail[0].MessageParts[0].Headers["Subject"], "The sent email had an incorrect subject");
            Assert.AreEqual(message, _server.ReceivedEmail[0].MessageParts[0].BodyData, "The send email had an incorrect message body");
        }
    }
}
