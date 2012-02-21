using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.ViewModels.Mail;
using System.IO;
using RazorEngine;
using MyJobLeads.DomainModel.Utilities;

namespace MyJobLeads.DomainModel.Mail
{
    public class FillPerfectMailController
    {
        public void SendPilotStudentLicenseEmail(FpOrgPilotUsedLicense license)
        {
            var model = Mapper.Map<FpOrgPilotUsedLicense, FillPerfectPilotStudentLicenceEmailViewModel>(license);
            string subject = "Welcome to the InterviewTools FillPerfect Pilot Program!";
            
            // Open the template and form the message
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string template = File.OpenText(baseDir + "bin/Mail/Templates/PilotStudentLicenseEmail.cshtml").ReadToEnd();
            string body = Razor.Parse(template, model);

            new EmailUtils().Send(license.Email, subject, body, true);
        }
    }
}
