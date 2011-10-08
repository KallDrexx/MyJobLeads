using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.ProcessParams.JobSearches;
using MyJobLeads.DomainModel.ViewModels.Exports;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using ClosedXML.Excel;
using System.IO;

namespace MyJobLeads.DomainModel.Processes
{
    public class JobSearchExportProcess : IProcess<ByJobSearchParams, JobsearchExportViewModel>
    {
        protected JobSearchByIdQuery _jobsearchQuery;

        public JobSearchExportProcess(JobSearchByIdQuery jobsearchQuery)
        {
            _jobsearchQuery = jobsearchQuery;
        }

        public JobsearchExportViewModel Execute(ByJobSearchParams procParams)
        {
            // Get the job search 
            var search = _jobsearchQuery.WithJobSearchId(procParams.JobSearchId).Execute();
            if (search == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), procParams.JobSearchId);

            // Form the workbook
            var workbook = new XLWorkbook();
            CreateSummaryWorksheet(workbook, search);
            CreateCompaniesWorksheet(workbook, search);
            CreateContactsWorksheet(workbook, search);
            CreateTaskWorksheet(workbook, search);
            CreatePositionWorksheet(workbook, search);

            // Save it to the memory string and return it
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var result = new JobsearchExportViewModel 
            {
                FileName = "MyLeadsJobsExport.xlsx", 
                ExportFileContents = stream.ToArray(),
                Mimetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
            stream.Close();

            return result;
        }

        protected void CreateCompaniesWorksheet(XLWorkbook workbook, JobSearch jobSearch)
        {
            // Create the worksheet
            var sheet = workbook.Worksheets.Add("Companies");

            // Write the headers
            sheet.Cell("A1").Value = "Name";
            sheet.Cell("B1").Value = "Phone";
            sheet.Cell("C1").Value = "City";
            sheet.Cell("D1").Value = "State";
            sheet.Cell("E1").Value = "Zip";
            sheet.Cell("F1").Value = "Status";
            sheet.Cell("G1").Value = "Notes";

            // Write the data 
            var companies = jobSearch.Companies.OrderBy(x => x.Name).ToList();
            for (int x = 0; x < companies.Count; x++)
            {
                var company = companies[x];
                int row = x + 2;

                sheet.Cell(row, "A").Value = company.Name;
                sheet.Cell(row, "B").Value = company.Phone;
                sheet.Cell(row, "C").Value = company.City;
                sheet.Cell(row, "D").Value = company.State;
                sheet.Cell(row, "E").Value = company.Zip;
                sheet.Cell(row, "F").Value = company.LeadStatus;
                sheet.Cell(row, "G").Value = company.Notes;

                sheet.Cell(row, "B").DataType = XLCellValues.Text;
                sheet.Cell(row, "E").DataType = XLCellValues.Text;
            }
        }

        protected void CreateContactsWorksheet(XLWorkbook workbook, JobSearch jobSearch)
        {
            // Create the worksheet
            var sheet = workbook.Worksheets.Add("Contacts");

            // Create the headers
            sheet.Cell(1, 1).Value = "Name";
            sheet.Cell(1, 2).Value = "Company";
            sheet.Cell(1, 3).Value = "Title";
            sheet.Cell(1, 4).Value = "Direct Phone";
            sheet.Cell(1, 5).Value = "Extension";
            sheet.Cell(1, 6).Value = "Mobile Phone";
            sheet.Cell(1, 7).Value = "Email";
            sheet.Cell(1, 8).Value = "Assistant";
            sheet.Cell(1, 9).Value = "Referred By";
            sheet.Cell(1, 10).Value = "Notes";

            // Write the data
            var contacts = jobSearch.Companies.SelectMany(x => x.Contacts).ToList();
            for (int x = 0; x < contacts.Count; x++)
            {
                int row = x + 2;

                sheet.Cell(row, 1).Value = contacts[x].Name;
                sheet.Cell(row, 2).Value = contacts[x].Company.Name;
                sheet.Cell(row, 3).Value = contacts[x].Title;
                sheet.Cell(row, 4).Value = contacts[x].DirectPhone;
                sheet.Cell(row, 5).Value = contacts[x].Extension;
                sheet.Cell(row, 6).Value = contacts[x].MobilePhone;
                sheet.Cell(row, 7).Value = contacts[x].Email;
                sheet.Cell(row, 8).Value = contacts[x].Assistant;
                sheet.Cell(row, 9).Value = contacts[x].ReferredBy;
                sheet.Cell(row, 10).Value = contacts[x].Notes;
            }
        }

        protected void CreateTaskWorksheet(XLWorkbook workbook, JobSearch jobSearch)
        {
            // Create the worksheet
            var sheet = workbook.Worksheets.Add("Tasks");

            // Write the headers
            sheet.Cell(1, 1).Value = "Name";
            sheet.Cell(1, 2).Value = "Company";
            sheet.Cell(1, 3).Value = "Contact";
            sheet.Cell(1, 4).Value = "Category";
            sheet.Cell(1, 5).Value = "Due Date";
            sheet.Cell(1, 6).Value = "Completion Date";
            sheet.Cell(1, 7).Value = "Notes";

            // Write the data
            var tasks = jobSearch.Companies.SelectMany(x => x.Tasks).ToList();
            for (int x = 0; x < tasks.Count; x++)
            {
                var task = tasks[x];
                int row = x + 2;

                sheet.Cell(row, 1).Value = task.Name;
                sheet.Cell(row, 2).Value = task.Company.Name;
                sheet.Cell(row, 3).Value = task.Contact != null ? task.Contact.Name : string.Empty;
                sheet.Cell(row, 4).Value = task.Category;
                sheet.Cell(row, 5).Value = task.TaskDate != null ? task.TaskDate.ToString() : string.Empty;
                sheet.Cell(row, 6).Value = task.CompletionDate != null ? task.CompletionDate.ToString() : string.Empty;
                sheet.Cell(row, 7).Value = tasks[x].Notes;
            }
        }

        protected void CreatePositionWorksheet(XLWorkbook workbook, JobSearch jobSearch)
        {
            // Create the worksheet
            var sheet = workbook.Worksheets.Add("Positions");

            // Write the headers
            sheet.Cell(1, 1).Value = "Title";
            sheet.Cell(1, 2).Value = "Company";
            sheet.Cell(1, 3).Value = "Has Applied";
            sheet.Cell(1, 4).Value = "Notes";

            // Write the data
            var positions = jobSearch.Companies.SelectMany(x => x.Positions).ToList();
            for (int x = 0; x < positions.Count; x++)
            {
                var position = positions[x];
                int row = x + 2;

                sheet.Cell(row, 1).Value = position.Title;
                sheet.Cell(row, 2).Value = position.Company.Name;
                sheet.Cell(row, 3).Value = position.HasApplied ? "Yes" : "No";
                sheet.Cell(row, 4).Value = position.Notes;
            }
        }

        protected void CreateSummaryWorksheet(XLWorkbook workbook, JobSearch jobSearch)
        {
            // Create the sheet
            var sheet = workbook.Worksheets.Add("Summary");

            // Write out the summary data
            sheet.Cell(1, 1).Value = "Owner:";
            sheet.Cell(1, 2).Value = jobSearch.User.FullName;

            sheet.Cell(2, 1).Value = "Export Made On:";
            sheet.Cell(2, 2).Value = DateTime.Now;

            sheet.Cell(3, 1).Value = "# of Companies:";
            sheet.Cell(3, 2).Value = jobSearch.Companies.Count;

            sheet.Cell(4, 1).Value = "# of Contacts:";
            sheet.Cell(4, 2).Value = jobSearch.Companies.SelectMany(x => x.Contacts).Count();

            sheet.Cell(5, 1).Value = "# of Tasks";
            sheet.Cell(5, 2).Value = jobSearch.Companies.SelectMany(x => x.Tasks).Count();

            sheet.Cell(6, 1).Value = "# of Positions";
            sheet.Cell(6, 2).Value = jobSearch.Companies.SelectMany(x => x.Positions).Count();

            sheet.Columns().AdjustToContents();
        }
    }
}
