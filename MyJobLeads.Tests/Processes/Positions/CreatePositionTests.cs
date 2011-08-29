using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Processes.Positions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.Tests.Processes.Positions
{
    [TestClass]
    public class CreatePositionTests : EFTestBase
    {
        [TestMethod]
        public void Can_Create_Position()
        {
            // Setup
            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();

            // Act
            process.Execute(new CreatePositionParams
            {
                CompanyId = company.Id,
                Title = "title",
                HasApplied = true,
                Notes = "Notes"
            });

            // Verify
            Position result = _context.Positions.Single();
            Assert.AreEqual("title", result.Title, "Position had an incorrect title");
            Assert.IsTrue(result.HasApplied, "Position had an incorrect HasApplied value");
            Assert.AreEqual("Notes", result.Notes, "Position had an incorrect note");
        }

        [TestMethod]
        public void Position_Creation_Returns_Display_View_Model()
        {
            // Setup
            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();

            // Act
            PositionDisplayViewModel result = process.Execute(new CreatePositionParams
            {
                CompanyId = company.Id,
                Title = "title",
                HasApplied = true,
                Notes = "Notes"
            });

            // Verify
            Assert.AreEqual("title", result.Title, "Position had an incorrect title");
            Assert.IsTrue(result.HasApplied, "Position had an incorrect HasApplied value");
            Assert.AreEqual("Notes", result.Notes, "Position had an incorrect note");
        }

        [TestMethod]
        public void Process_Throws_EntityNotFoundException_When_Company_Not_Found()
        {
            // Setup
            IProcess<CreatePositionParams, PositionDisplayViewModel> process = new PositionProcesses(_context);
            Company company = new Company();
            _context.Companies.Add(company);
            _context.SaveChanges();
            int id = company.Id + 1;

            // Act
            try
            {
                process.Execute(new CreatePositionParams { CompanyId = id });
                Assert.Fail("No exception was thrown");
            }

            // Verify
            catch (MJLEntityNotFoundException ex)
            {
                Assert.AreEqual(typeof(Company), ex.EntityType, "MJLEntityNotFoundException's entity type was incorrect");
                Assert.AreEqual(id.ToString(), ex.IdValue, "MJLEntityNotFoundException's id value was incorrect");
            }
        }
    }
}