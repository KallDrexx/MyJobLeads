using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers.Validation;

namespace MyJobLeads.Tests.Providers.Validators
{
    [TestClass]
    public class TaskValidatorTests
    {
        private Task _task;

        [TestInitialize]
        public void Setup()
        {
            _task = new Task
            {
                Name = "Name"
            };
        }

        [TestMethod]
        public void Correct_Task_Passes_Validation()
        {
            // Act
            bool result = new TaskValidator().Validate(_task).IsValid;

            // Verify
            Assert.IsTrue(result, "Task was incorrectly marked as invalid");
        }

        [TestMethod]
        public void Task_With_Empty_Name_Is_Invalid()
        {
            // Setup
            _task.Name = " ";

            // Act
            bool result = new TaskValidator().Validate(_task).IsValid;

            // Verify
            Assert.IsFalse(result, "Task was incorrectly marked as valid");
        }

        [TestMethod]
        public void Task_With_Null_Name_Is_Invalid()
        {
            // Setup
            _task.Name = null;

            // Act
            bool result = new TaskValidator().Validate(_task).IsValid;

            // Verify
            Assert.IsFalse(result, "Task was incorrectly marked as valid");
        }
    }
}
