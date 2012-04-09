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
            var user = new User { FillPerfectKey = Guid.NewGuid() };
            var order = new Order();
            order.FillPerfectLicenses.Add(new FpUserLicense
            {
                EffectiveDate = DateTime.Now.AddMonths(-1),
                ExpirationDate = DateTime.Now.AddMonths(1)
            });

            user.OwnedOrders.Add(order);
            _context.Users.Add(user);
        }

        [TestMethod]
        public void LicenseRequestReturnsInvalidKeyWhenNoUserHasSpecifiedKey()
        {

            // Act
            var result = _getKeyProc.Execute(new GetFillPerfectLicenseByKeyParams { FillPerfectKey = Guid.NewGuid() });

            // Verify
            Assert.AreEqual(FillPerfectLicenseError.InvalidKey, result.Error, "Incorrect license error returned");
        }
    }
}
