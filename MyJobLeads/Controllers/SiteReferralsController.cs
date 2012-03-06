using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Controllers
{
    public partial class SiteReferralsController : MyJobLeadsBaseController
    {
        public SiteReferralsController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Log(string code, string url) 
        {
            _context.SiteReferrals.Add(new SiteReferral
            {
                ReferralCode = code,
                IpAddress = Request.UserHostAddress,
                Url = url,
                Date = DateTime.Now
            });
            _context.SaveChanges();

            return new EmptyResult();
        }

        public virtual ActionResult AddCode(string code, string desc)
        {
            _context.SiteReferralCodes.Add(new SiteReferralCode
            {
                Code = code,
                GivenTo = desc
            });
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
