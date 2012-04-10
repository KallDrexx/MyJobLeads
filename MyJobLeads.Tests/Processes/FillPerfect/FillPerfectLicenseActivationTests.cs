using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.Entities.Ordering;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.FillPerfect;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;
using MyJobLeads.DomainModel.Processes.FillPerfect;
using MyJobLeads.DomainModel.Enums.FillPerfect;

namespace MyJobLeads.Tests.Processes.FillPerfect
{
    [TestClass]
    public class FillPerfectLicenseActivationTests : EFTestBase
    {
        protected IProcess<ActivateFillPerfectKeyParams, FillPerfectKeyActivationViewModel> _activationProc;
        protected User _user;

        [TestInitialize]
        public void Init()
        {
            _activationProc = new FillPerfectLicenseProcesses(_context);

            _user = new User { FillPerfectKey = Guid.NewGuid(), FullName = "Test Name", Email = "test@blah.com" };
            var order = new Order { OrderDate = DateTime.Now };
            order.FillPerfectLicenses.Add(new FpUserLicense
            {
                EffectiveDate = DateTime.Now.AddMonths(-1),
                ExpirationDate = DateTime.Now.AddMonths(1),
                LicenseType = FillPerfectLicenseType.IndividualPaid
            });

            _user.OwnedOrders.Add(order);
            _context.Users.Add(_user);
            _context.SaveChanges();
        }

        [TestMethod]
        public void InvalidKey_Result_When_FillPerfect_Key_Doesnt_Match_For_Any_User()
        {
            // Act
            var result = _activationProc.Execute(new ActivateFillPerfectKeyParams { FillPerfectKey = Guid.NewGuid(), MachineId = "1234" });

            // Verify
            Assert.AreEqual(FillPerfectActivationResult.InvalidKey, result.Result, "Process returned an incorrect result");
        }

        [TestMethod]
        public void KeyAlreadyActivated_Result_When_Key_Already_Activated()
        {
            // Setup 
            _user.OwnedOrders.First().FillPerfectLicenses.First().ActivatedComputerId = "abcd";
            _context.SaveChanges();

            // Act
            var result = _activationProc.Execute(new ActivateFillPerfectKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey, MachineId = "1234" });

            // Verify
            Assert.AreEqual(FillPerfectActivationResult.KeyAlreadyActivated, result.Result, "Activation result value was incorrect");
        }

        [TestMethod]
        public void Successful_Activation_When_Key_Isnt_Activated()
        {
            // Act
            var result = _activationProc.Execute(new ActivateFillPerfectKeyParams { FillPerfectKey = (Guid)_user.FillPerfectKey, MachineId = "1234" });

            // Verify
            Assert.AreEqual(FillPerfectActivationResult.ActivationSuccessful, result.Result, "Incorrect activation result was returned");
            Assert.AreEqual("1234", _user.OwnedOrders.First().FillPerfectLicenses.First().ActivatedComputerId, "Activated computer id was incorrect");
        }
    }
}
