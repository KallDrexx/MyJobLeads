using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.Areas.FillPerfect.Models.Pilot;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    [RequiresOrganizationAdmin]
    public partial class OrgPilotDashboardController : MyJobLeadsBaseController
    {
        public OrgPilotDashboardController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Index()
        {
            var org = _context.Users
                              .Where(x => x.Id == CurrentUserId)
                              .Select(x => x.Organization)
                              .Include(x => x.FpUsedPilotLicenses)
                              .SingleOrDefault();

            if (string.IsNullOrWhiteSpace(org.FillPerfectPilotKey))
                return RedirectToAction(MVC.Organization.Dashboard.Index());

            var model = new OrgDashboardViewModel
            {
                AlottedLicenseCount = org.FpPilotLicenseCount,
                RemainingLicenseCount = org.FpPilotLicenseCount - org.FpUsedPilotLicenses.Count
            };

            foreach (var user in org.FpUsedPilotLicenses)
            {
                model.LicensedUsers.Add(new OrgDashboardViewModel.PilotLicenseUser
                {
                    Name = user.FullName,
                    Email = user.Email,
                    GrantedDate = user.LicenseGrantDate
                });
            }

            return View(model);
        }
    }
}
