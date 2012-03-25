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
                                    .OrderBy(x => x.IpAddress)
                                    .Select(x => new
                                    {
                                        Ip = x.IpAddress,
                                        Name = x.ReferralCode
                                    })
                                    .Distinct()
                                    .ToList();

            // Format the data into excel
            var workbook = new XLWorkbook();
            var ipMapSheet = workbook.Worksheets.Add("Known Users");
            var activitySheet = workbook.Worksheets.Add("Activity");

            // Create the ip mapping sheet
            ipMapSheet.Cell("A1").Value = "IP Address";
            ipMapSheet.Cell("B1").Value = "User Code";

            for (int x = 0; x < knownUsers.Count; x++)
            {
                int row = x + 2;
                ipMapSheet.Cell(row, "A").Value = knownUsers[x].Ip;
                ipMapSheet.Cell(row, "B").Value = knownUsers[x].Name;
                ipMapSheet.Cell(row, "A").DataType = XLCellValues.Text;
                ipMapSheet.Cell(row, "B").DataType = XLCellValues.Text;
            }

            // Create the activity sheet
            activitySheet.Cells("A1").Value = "Date";
            activitySheet.Cells("B1").Value = "IP Address";
            activitySheet.Cells("C1").Value = "Site Address";

            for (int x = 0; x < activity.Count; x++)
            {
                int row = x + 2;
                activitySheet.Cell(row, "A").Value = activity[x].Date;
                activitySheet.Cell(row, "B").Value = activity[x].IpAddress;
                activitySheet.Cell(row, "C").Value = activity[x].Url;

                activitySheet.Cell(row, "A").DataType = XLCellValues.DateTime;
                activitySheet.Cell(row, "B").DataType = XLCellValues.Text;
                activitySheet.Cell(row, "C").DataType = XLCellValues.Text;
            }

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
