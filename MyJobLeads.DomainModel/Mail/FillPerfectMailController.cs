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
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;

namespace MyJobLeads.DomainModel.Mail
{
    public class FillPerfectMailController
    {
        public void SendPilotStudentLicenseEmail(FpOrgPilotUsedLicense license)
        {
            var model = Mapper.Map<FpOrgPilotUsedLicense, FillPerfectPilotStudentLicenceEmailViewModel>(license);
            string subject = "Welcome to the InterviewTools FillPerfect Pilot Program!";
            
            // Open the template and form the message
            string body = ParseTemplate("Mail/Templates/FillPerfect/PilotStudentLicenseEmail.cshtml", model);

            new EmailUtils().Send(license.Email, subject, body, true);
        }

        public void SendContactReplyEmail(SendFpReplyParams model)
        {
            string body = ParseTemplate("Mail/Templates/FillPerfect/FpContactReplyEmail.cshtml", model);
            new EmailUtils().Send(model.ToAddress, model.Subject, body, true, model.FromAddress);
        }

        protected string ParseTemplate<TModel>(string templateFilename, TModel model) where TModel : class
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string template = File.OpenText(string.Concat(baseDir, "bin/", templateFilename)).ReadToEnd();
            return Razor.Parse(template, model);
        }
    }
}
