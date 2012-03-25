using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.ViewModels.General;

namespace MyJobLeads.DomainModel.Processes.Admin
{
    public class SiteActivityProcesses : IProcess<ExportSiteActivityReportParams, FileContentViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public SiteActivityProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public FileContentViewModel Execute(ExportSiteActivityReportParams procParams)
        {
            // Get site activity and names for the ip addresses
            var activity = _context.SiteReferrals
                                   .Where(x => x.Date >= procParams.StartDate && x.Date <= procParams.EndDate)
                                   .OrderBy(x => x.Date)
                                   .ToList();

            var ips = activity.Select(x => x.IpAddress).Distinct().ToList();

            var knownUsers = _context.SiteReferrals
                                    .Where(x => ips.Contains(x.IpAddress))
                                    .Where(x => !string.IsNullOrEmpty(x.ReferralCode))
                                    .OrderByDescending(x => x.Date)
                                    .Select(x => new
                                    {
                                        Ip = x.IpAddress,
                                        Name = x.ReferralCode
                                    })
                                    .Distinct()
                                    .ToList();

            // Format the data into excel
            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Activity");

            // Create the activity sheet
            sheet.Cells("A1").Value = "Date";
            sheet.Cells("B1").Value = "IP Address";
            sheet.Cells("C1").Value = "Inferred user";
            sheet.Cells("D1").Value = "Is Email Click?";
            sheet.Cells("E1").Value = "Site Address";

            int row = 2;
            for (int x = 0; x < activity.Count; x++)
            {
                if (knownUsers.Any(y => y.Ip == activity[x].IpAddress))
                {
                    sheet.Cell(row, "A").Value = activity[x].Date;
                    sheet.Cell(row, "B").Value = activity[x].IpAddress;
                    sheet.Cell(row, "C").Value = knownUsers.Where(y => y.Ip == activity[x].IpAddress).Select(y => y.Name).First();
                    sheet.Cell(row, "D").Value = activity[x].Url.Contains("refId=");
                    sheet.Cell(row, "E").Value = activity[x].Url;

                    sheet.Cell(row, "A").DataType = XLCellValues.DateTime;
                    sheet.Cell(row, "D").DataType = XLCellValues.Boolean;

                    row++;
                }
            }

            sheet.Rows().AdjustToContents(1, 4);

            // Save it to the memory string and return it
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var result = new FileContentViewModel
            {
                FileName = "SiteActivityReport.xlsx",
                ExportFileContents = stream.ToArray(),
                Mimetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
            stream.Close();

            return result;
        }
    }
}
