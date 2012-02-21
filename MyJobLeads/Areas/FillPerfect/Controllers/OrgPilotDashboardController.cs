using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.Areas.FillPerfect.Models.Pilot;
using MyJobLeads.DomainModel;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.Mail;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    [RequiresOrganizationAdmin]
    public partial class OrgPilotDashboardController : MyJobLeadsBaseController
    {
        public OrgPilotDashboardController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Index(bool licenseSent = false)
        {
            var org = _context.Users
                              .Where(x => x.Id == CurrentUserId)
                              .Select(x => x.Organization)
                              .Include(x => x.FpUsedPilotLicenses)
                              .SingleOrDefault();

            if (string.IsNullOrWhiteSpace(org.FillPerfectPilotKey))
                return RedirectToAction(MVC.Organization.Dashboard.Index());

            if (licenseSent)
                ModelState.AddModelError("", "The license was successfully sent to your student");

            return View(CreateDashboardViewModel(org));
        }

        [HttpPost]
        public virtual ActionResult SendPilotLicense(string name, string email)
        {
            var org = _context.Users
                              .Where(x => x.Id == CurrentUserId)
                              .Select(x => x.Organization)
                              .Include(x => x.FpUsedPilotLicenses)
                              .SingleOrDefault();

            if (string.IsNullOrWhiteSpace(org.FillPerfectPilotKey))
                return RedirectToAction(MVC.Organization.Dashboard.Index());

            // Make sure they have a free license
            if (org.FpUsedPilotLicenses.Count >= org.FpPilotLicenseCount)
                ModelState.AddModelError("", "You do not have any free licenses available");

            // Do some simple validation
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                ModelState.AddModelError("", "The student name or email fields are required");

            if (ModelState.IsValid)
            {
                // Set them up in the database
                var license = new DomainModel.Entities.FillPerfect.FpOrgPilotUsedLicense
                {
                    FullName = name.Trim(),
                    Email = email.Trim(),
                    LicenseGrantDate = DateTime.Now
                };

                license.Organization = org;
                _context.FpOrgPilotUsedLicenses.Add(license);
                _context.SaveChanges();

                // Email the user
                SendLicenseToUser(license);
            }

            if (!ModelState.IsValid)
                return View(MVC.FillPerfect.OrgPilotDashboard.Views.Index, CreateDashboardViewModel(org));
            else
                return RedirectToAction(MVC.FillPerfect.OrgPilotDashboard.Index(true));
        }

        public virtual ActionResult ResendLicense(int licensedUserId)
        {
            var user = _context.FpOrgPilotUsedLicenses
                               .Where(x => x.Id == licensedUserId)
                               .Include(x => x.Organization)
                               .FirstOrDefault();

            SendLicenseToUser(user);
            return RedirectToAction(MVC.FillPerfect.OrgPilotDashboard.Index(true));                   
        }

        protected OrgDashboardViewModel CreateDashboardViewModel(MyJobLeads.DomainModel.Entities.Organization org)
        {
            var model = new OrgDashboardViewModel
            {
                AlottedLicenseCount = org.FpPilotLicenseCount,
                RemainingLicenseCount = org.FpPilotLicenseCount - org.FpUsedPilotLicenses.Count
            };

            foreach (var user in org.FpUsedPilotLicenses)
            {
                model.LicensedUsers.Add(new OrgDashboardViewModel.PilotLicenseUser
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Email = user.Email,
                    GrantedDate = user.LicenseGrantDate
                });
            }

            return model;
        }

        protected void SendLicenseToUser(FpOrgPilotUsedLicense user)
        {
            new FillPerfectMailController().SendPilotStudentLicenseEmail(user);
        }
    }
}
