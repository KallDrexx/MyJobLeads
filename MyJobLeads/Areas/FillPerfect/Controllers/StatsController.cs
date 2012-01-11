using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    public class StatsController : MyJobLeadsBaseController
    {
        public StatsController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public JsonResult JobsApplied(string userId, int applyCount)
        {
            _context.FpJobApplyBasicStats.Add(new FpJobApplyBasicStat
            {
                UserId = userId,
                ApplyCount = applyCount,
                PostedDate = DateTime.Now
            });
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
