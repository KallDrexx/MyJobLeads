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

namespace MyJobLeads.Tests.Processes.FillPerfect
{
    [TestClass]
    public class FillPerfectLicenseTests : EFTestBase
    {
        protected IProcess<GetFillPerfectLicenseByKeyParams, FillPerfectLicenseViewModel> _getKeyProc;
        protected User _user;

        [TestInitialize]
        public void Init()
        {
            _getKeyProc = new FillPerfectLicenseProcesses(_context);

            // Db Entities
            _user = new User { FillPerfectKey = Guid.NewGuid() };
            var order = new Order { OrderDate = DateTime.Now };
            order.FillPerfectLicenses.Add(new FpUserLicense
            {
                ActivatedComputerId = "1234",
                EffectiveDate = DateTime.Now.AddMonths(-1),
                ExpirationDate = DateTime.Now.AddMonths(1)
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
        }
    }
}
