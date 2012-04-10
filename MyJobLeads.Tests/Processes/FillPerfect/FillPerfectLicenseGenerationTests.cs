using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Ordering;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.Processes.FillPerfect;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.FillPerfect;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;
using MyJobLeads.DomainModel.Enums;
using System.Xml.Linq;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;

namespace MyJobLeads.Tests.Processes.FillPerfect
{
    [TestClass]
    public class FillPerfectLicenseGenerationTests : EFTestBase
    {
        protected IProcess<GetFillPerfectLicenseByKeyParams, FillPerfectLicenseViewModel> _getKeyProc;
        protected User _user;

        [TestInitialize]
        public void Init()
        {
            _getKeyProc = new FillPerfectLicenseProcesses(_context);

            // Db Entities
            _user = new User { FillPerfectKey = Guid.NewGuid(), FullName = "Test Name", Email = "test@blah.com" };
            var order = new Order { OrderDate = DateTime.Now };
            order.FillPerfectLicenses.Add(new FpUserLicense
            {
                ActivatedComputerId = "1234",
                EffectiveDate = DateTime.Now.AddMonths(-1),
                ExpirationDate = DateTime.Now.AddMonths(1),
                LicenseType = FillPerfectLicenseType.IndividualPaid
            });

            _user.OwnedOrders.Add(order);
            _context.Users.Add(_user);
            _context.SaveChanges();
        }

        [TestMethod]
        public void License_Request_Returns_InvalidKey_When_No_User_Has_Specified_Key()
        {
            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = Guid.NewGuid() });

            // Verify
            Assert.AreEqual(FillPerfectLicenseError.InvalidKey, result.Error, "Incorrect license error returned");
        }

        [TestMethod]
        public void License_Request_Returns_Not_Activated_When_License_Has_No_ActivatedMachineId()
        {
            // Setup
            _user.OwnedOrders
                 .SelectMany(x => x.FillPerfectLicenses)
                 .OrderByDescending(x => x.EffectiveDate)
                 .First().ActivatedComputerId = null;
            _context.SaveChanges();

            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey });

            // Verify
            Assert.AreEqual(FillPerfectLicenseError.KeyNotActivated, result.Error, "Incorrect license error returned");
        }

        [TestMethod]
        public void License_Request_Returns_No_License_Error_When_User_Has_Activated_Licene()
        {
            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey });

            // Verify
            Assert.AreEqual(FillPerfectLicenseError.None, result.Error, "Incorrect license error returned");
        }

        [TestMethod]
        public void License_Request_Returns_Time_Of_Request()
        {
            // Setup
            DateTime start = DateTime.Now;

            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey });

            // Verify
            DateTime end = DateTime.Now;
            Assert.IsTrue(result.RequestDate >= start && result.RequestDate <= end, "Request date was incorrect");
        }

        [TestMethod]
        public void License_Contains_Expected_Xml()
        {
            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey });

            // Verify
            var license = _user.OwnedOrders
                               .SelectMany(x => x.FillPerfectLicenses)
                               .OrderByDescending(x => x.EffectiveDate)
                               .First();

            Assert.IsFalse(string.IsNullOrWhiteSpace(result.LicenseXml), "License XML is null or empty");
            
            // Test the xml
            var xml = XDocument.Parse(result.LicenseXml);
            Assert.IsTrue(xml.Root.Name == "FillPerfectLicense", "Root node was incorrect");
            Assert.AreEqual(1, xml.Descendants("LicenseType").Count(), "LicenseType descendent count incorrect");
            Assert.AreEqual("IndividualPaid", xml.Descendants("LicenseType").First().Value, "License type was incorrect");
            Assert.AreEqual(1, xml.Descendants("LicensedFor").Count(), "LicensedFor node count incorrect");
            Assert.AreEqual(_user.FullName, xml.Descendants("LicensedFor").First().Value, "LicensedFor value was incorrect");
            Assert.AreEqual(1, xml.Descendants("LicenseForMachine").Count(), "LicenseForMachine node count was incorrect");
            Assert.AreEqual("1234", xml.Descendants("LicenseForMachine").First().Value, "LicenseForMachine value was incorrect");
            Assert.AreEqual(1, xml.Descendants("EffectiveDate").Count(), "EffectiveDate node count was incorrect");
            Assert.AreEqual(license.EffectiveDate.ToLongDateString(), xml.Descendants("EffectiveDate").First().Value, "EffectiveDate value was incorrect");
            Assert.AreEqual(1, xml.Descendants("ExpirationDate").Count(), "ExpirationDate node count was incorrect");
            Assert.AreEqual(license.ExpirationDate.ToLongDateString(), xml.Descendants("ExpirationDate").First().Value, "ExpirationDate value was incorrect");
            Assert.AreEqual(1, xml.Descendants("Email").Count(), "Email node count was incorrect");
            Assert.AreEqual(_user.Email, xml.Descendants("Email").First().Value, "Email value was incorrect");
        }

        [TestMethod]
        public void License_Contains_Xml_Signature()
        {
            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey });

            // Verify
            var xml = XDocument.Parse(result.LicenseXml);
            XNamespace ns = "http://www.w3.org/2000/09/xmldsig#";
            Assert.AreEqual(1, xml.Descendants(ns + "Signature").Count(), "Incorrect number of signature nodes found");
        }

        [TestMethod]
        public void License_Can_Be_Verified()
        {
            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey });

            // Verify
            var xml = XDocument.Parse(result.LicenseXml);
            var xmlDocument = new XmlDocument();
            using (var reader = xml.CreateReader())
                xmlDocument.Load(reader);

            var key = new RSACryptoServiceProvider();
            key.FromXmlString(result.KeyXml);

            var signedXml = new SignedXml(xmlDocument);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");
            signedXml.LoadXml((XmlElement)nodeList[0]);

            Assert.IsTrue(signedXml.CheckSignature(key), "Signed xml failed verification");
        }
    }
}
