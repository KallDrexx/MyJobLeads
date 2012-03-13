using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Areas.FillPerfect.Models.UpdateOrgTrialKey;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    [RequiresSiteAdmin]
    public partial class UpdateOrgTrialKeyController : MyJobLeadsBaseController
    {
        public UpdateOrgTrialKeyController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Index()
        {
            var model = new OrgSelectionViewModel();
            model.FormOrgList(_context.Organizations.ToList());

            return View(model);
        }

        public virtual ActionResult Edit(OrgSelectionViewModel selectedOrg)
        {
            var org = _context.Organizations
                              .Where(x => x.Id == selectedOrg.SelectedOrgId)
                              .Single();

            var model = new OrgTrialViewModel
            {
                OrganizationId = org.Id,
                OrganizationName = org.Name,
                FillPerfectKey = org.FillPerfectPilotKey,
                TotaltrialCount = org.FpPilotLicenseCount,
                OrigTrialCount = org.FpPilotLicenseCount
            };

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(OrgTrialViewModel model)
        {
            if (model.TotaltrialCount == 0 && !model.ZeroCountConfirm)
            {
                model.ZeroCountConfirm = true;
                ModelState.AddModelError("", "Trial license count set to zero.  Submit again to confirm");
            }
            else if (model.ZeroCountConfirm)
                model.ZeroCountConfirm = false;

            if (!ModelState.IsValid)
                return View(model);

            var org = _context.Organizations
                              .Where(x => x.Id == model.OrganizationId)
                              .Single();

            org.FillPerfectPilotKey = model.FillPerfectKey;
            org.FpPilotLicenseCount = model.TotaltrialCount;
            _context.SaveChanges();

            return RedirectToAction(MVC.Admin.Dashboard.Index());
        }
    }
}
