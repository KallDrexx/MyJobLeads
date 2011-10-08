using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Exports;
using MyJobLeads.DomainModel.ProcessParams.JobSearches;
using MyJobLeads.DomainModel.Processes;
using MyJobLeads.DomainModel.Queries.JobSearches;
using Moq;
using MyJobLeads.DomainModel.Exceptions;
using System.IO;
using ClosedXML.Excel;

namespace MyJobLeads.Tests.Processes.Export
{
    [TestClass]
    public class JobSearchExportProcessTests : EFTestBase
    {
        protected const string COMPANY_SHEET_NAME = "Companies";
        protected IProcess<ByJobSearchParams, JobsearchExportViewModel> _process;
        protected Mock<JobSearchByIdQuery> _jsQueryMock;
        protected Company _comp1, _comp2;

        [TestInitialize]
        public void InitializeTestEntities()
        {
            var search = new JobSearch();
            _comp1 = new Company
            {
                Name = "company",
                Phone = "555-333-2323",
                State = "FL",
                Zip = "33445",
                City = "City",
                LeadStatus = "Status1",
                Notes = "notes 1",
                JobSearch = search
            };

            _comp2 = new Company
            {
                Name = "company2",
                Phone = "666-333-2323",
                State = "NY",
                Zip = "23456",
                City = "City2",
                LeadStatus = "Status2",
                Notes = "notes 2",
                JobSearch = search
            };

            _context.Companies.Add(_comp1);
            _context.Companies.Add(_comp2);
            _context.SaveChanges();

            // Create the export process class
            _jsQueryMock = new Mock<JobSearchByIdQuery>(_unitOfWork);
            _jsQueryMock.Setup(x => x.Execute()).Returns(search);
            _jsQueryMock.Setup(x => x.WithJobSearchId(It.IsAny<int>())).Returns(_jsQueryMock.Object);
            _process = new JobSearchExportProcess(_jsQueryMock.Object);
        }

        public XLWorkbook GetWorkbook()
        {
            var model = _process.Execute(new ByJobSearchParams());
            var stream = new MemoryStream(model.ExportFileContents);
            return new XLWorkbook(stream);
        }

        [TestMethod]
        public void Process_Returns_A_Non_Null_Export()
        {
            // Act
            JobsearchExportViewModel result = _process.Execute(new ByJobSearchParams());

            // Verify
            Assert.IsNotNull(result, "Process returned a null result");
        }

        [TestMethod]
        public void Process_Throws_EntityNotFoundException_When_JobSearch_Doesnt_Exist()
        {
            // Setup
            int id = 5;
            _jsQueryMock.Setup(x => x.Execute()).Returns((JobSearch)null);

            // Act
            try
            {
                _process.Execute(new ByJobSearchParams { JobSearchId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(JobSearch), ex.EntityType, "Exception's entity type value was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "Exception's id value was incorrect");
            }
        }

        [TestMethod]
        public void Process_Requests_Specified_JobSearch()
        {
            // Act
            _process.Execute(new ByJobSearchParams { JobSearchId = 15 });

            // Verify
            _jsQueryMock.Verify(x => x.WithJobSearchId(15));
        }

        [TestMethod]
        public void Export_Contains_Valid_Excel_Worksheet()
        {
            // Act
            var workbook = GetWorkbook();

            // Verify
            Assert.IsNotNull(workbook, "Workbook was null");
        }

        [TestMethod]
        public void Export_Contains_Companies_Worksheet()
        {
            // Act
            var workbook = GetWorkbook();

            // Verify
            Assert.IsTrue(workbook.Worksheets.Any(x => x.Name == COMPANY_SHEET_NAME), "No company worksheet was found");
        }

        [TestMethod]
        public void Company_Sheet_Contains_Correct_Headers()
        {
            // Setup
            var workbook = GetWorkbook();

            // Act
            var sheet = workbook.Worksheets.Where(x => x.Name == COMPANY_SHEET_NAME).Single();

            // Verify
            Assert.AreEqual("Name", sheet.Cell(1, 1).Value, "First header was not correct");
            Assert.AreEqual("Phone", sheet.Cell(1, 2).Value, "Second header was not correct");
            Assert.AreEqual("City", sheet.Cell(1, 3).Value, "Third header was not correct");
            Assert.AreEqual("State", sheet.Cell(1, 4).Value, "Fourth header was not correct");
            Assert.AreEqual("Zip", sheet.Cell(1, 5).Value, "Fifth header was not correct");
            Assert.AreEqual("Status", sheet.Cell(1, 6).Value, "Sixth header was not correct");
            Assert.AreEqual("Notes", sheet.Cell(1, 7).Value, "Seventh header was not correct");
        }

        [TestMethod]
        public void Company_Sheet_Contains_Correct_Company_Data()
        {
            // Setup
            var workbook = GetWorkbook();

            // Act
            var sheet = workbook.Worksheets.Where(x => x.Name == COMPANY_SHEET_NAME).Single();

            // Verify
            Assert.AreEqual(_comp1.Name, sheet.Cell(2, 1).Value, "Company1's name was incorrect");
            Assert.AreEqual(_comp1.Phone, sheet.Cell(2, 2).Value, "Company1's phone was incorrect");
            Assert.AreEqual(_comp1.City, sheet.Cell(2, 3).Value, "Company1's city was incorrect");
            Assert.AreEqual(_comp1.State, sheet.Cell(2, 4).Value, "Company1's state was incorrect");
            Assert.AreEqual(_comp1.Zip, sheet.Cell(2, 5).Value, "Company1's zip was incorrect");
            Assert.AreEqual(_comp1.LeadStatus, sheet.Cell(2, 6).Value, "Company1's status was incorrect");
            Assert.AreEqual(_comp1.Notes, sheet.Cell(2, 7).Value, "Company1's notes was incorrect");

            Assert.AreEqual(_comp2.Name, sheet.Cell(3, 1).Value, "Company2's name was incorrect");
            Assert.AreEqual(_comp2.Phone, sheet.Cell(3, 2).Value, "Company2's phone was incorrect");
            Assert.AreEqual(_comp2.City, sheet.Cell(3, 3).Value, "Company2's city was incorrect");
            Assert.AreEqual(_comp2.State, sheet.Cell(3, 4).Value, "Company2's state was incorrect");
            Assert.AreEqual(_comp2.Zip, sheet.Cell(3, 5).Value, "Company2's zip was incorrect");
            Assert.AreEqual(_comp2.LeadStatus, sheet.Cell(3, 6).Value, "Company2's status was incorrect");
            Assert.AreEqual(_comp2.Notes, sheet.Cell(3, 7).Value, "Company2's notes was incorrect");

            Assert.AreEqual("", sheet.Cell(4, 1).Value, "The 4th row was not empty");
        }
    }
}
