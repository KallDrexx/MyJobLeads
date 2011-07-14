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
        [TestMethod]
        public void Jobsearch_with_Empty_Name_Is_Invalid()
        {
            // Setup
            JobSearch search = new JobSearch { Name = " " };

            // Act
            bool result = new JobSearchValidator().Validate(search).IsValid;

            // Verify
            Assert.IsFalse(result, "Job search with an empty name was marked as valid");
        }

        [TestMethod]
        public void Jobsearch_With_Null_Name_Is_Invalid()
        {
            // Setup
            JobSearch search = new JobSearch();

            // Act
            bool result = new JobSearchValidator().Validate(search).IsValid;

            // Verify
            Assert.IsFalse(result, "Job search with an empty name was marked as valid");
        }
    }
}
