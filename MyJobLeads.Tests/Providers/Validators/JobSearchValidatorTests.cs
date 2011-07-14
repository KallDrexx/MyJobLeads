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
    public class JobSearchValidatorTests
    {
        private JobSearch _search;

        [TestInitialize]
        public void InitializeEntities()
        {
            _search = new JobSearch
            {
                Name = "Name"
            };
        }

        [TestMethod]
        public void Jobsearch_Is_Valid()
        {
            // Act
            bool result = new JobSearchValidator().Validate(_search).IsValid;

            // Verify
            Assert.IsTrue(result, "The job search was not marked as valid");
        }

        [TestMethod]
        public void Jobsearch_with_Empty_Name_Is_Invalid()
        {
            // Setup
            _search.Name = " ";

            // Act
            bool result = new JobSearchValidator().Validate(_search).IsValid;

            // Verify
            Assert.IsFalse(result, "Job search with an empty name was marked as valid");
        }

        [TestMethod]
        public void Jobsearch_With_Null_Name_Is_Invalid()
        {
            // Setup
            _search.Name = null;

            // Act
            bool result = new JobSearchValidator().Validate(_search).IsValid;

            // Verify
            Assert.IsFalse(result, "Job search with an empty name was marked as valid");
        }
    }
}
