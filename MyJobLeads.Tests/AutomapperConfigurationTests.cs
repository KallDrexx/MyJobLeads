using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.EntityMapping;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Json.Jigsaw;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;

namespace MyJobLeads.Tests
{
    public class UnitTestEntityViewModel
    {
        public int Id { get; set; }
        public string Text2 { get; set; }
    }

    public class TestProjectionMapping : IEntityMapConfiguration
    {
        public void ConfigureMapping()
        {
            Mapper.CreateMap<UnitTestEntity, UnitTestEntityViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text2, opt => opt.MapFrom(src => src.Text));
        }
    }

    [TestClass]
    public class AutomapperConfigurationTests
    {
        [TestMethod]
        public void Loader_Loads_All_Configurations_In_Assembly()
        {
            // Setup
            UnitTestEntity entity = new UnitTestEntity { Id = 2, Text = "Test 1234" };

            // Act
            EntityMapLoader.LoadEntityMappings();

            // Verify
            UnitTestEntityViewModel result = Mapper.Map<UnitTestEntity, UnitTestEntityViewModel>(entity);
            Assert.AreEqual(entity.Id, result.Id, "View model's ID value did not match");
            Assert.AreEqual(entity.Text, result.Text2, "View model's text value did not match");
        }

        [TestMethod]
        public void Automapper_Configurations_Are_Valid()
        {
            // Act
            EntityMapLoader.LoadEntityMappings();

            // Validate
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Automapper_Contact_Details_Json_Can_Be_Mapped()
        {
            // Setup
            EntityMapLoader.LoadEntityMappings();
            DateTime testDate = DateTime.Now;

            var json = new ContactDetailsJson
            {
                CompanyName = "company",
                ContactId = "12345",
                FirstName = "first",
                LastName = "last",
                Owned = true,
                Title = "title",
                UpdatedDate = testDate
            };

            // Act
            var result = Mapper.Map<ContactDetailsJson, ExternalContactSearchResultsViewModel.ContactResultViewModel>(json);

            // Verify
            Assert.IsNotNull(result, "result was null");
            Assert.AreEqual("company", result.Company);
            Assert.AreEqual("12345", result.ContactId);
            Assert.AreEqual("first", result.FirstName);
            Assert.AreEqual("last", result.LastName);
            Assert.AreEqual(true, result.HasAccess);
            Assert.AreEqual("title", result.Headline);
            Assert.AreEqual(testDate, result.LastUpdatedDate);
        }
    }
}
