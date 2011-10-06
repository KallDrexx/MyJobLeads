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
            CreateCompaniesWorksheet(workbook, search);

            // Save it to the memory string and return it
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var result = new JobsearchExportViewModel { FileName = "JobSearchExport.xlsx", ExportFileContents = stream.ToArray() };
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
    }
}
