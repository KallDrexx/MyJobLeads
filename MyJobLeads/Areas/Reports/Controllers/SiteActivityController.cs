using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Areas.Reports.Models.SiteActivityViewModels;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.Reports.Controllers
{
    [RequiresSiteAdmin]
    public partial class SiteActivityController : MyJobLeadsBaseController
    {
        public SiteActivityController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Index()
        {
            var users = _context.SiteReferrals
                                .Where(x => !string.IsNullOrEmpty(x.ReferralCode))
                                .Select(x => new
                                {
                                    Ip = x.IpAddress,
                                    Name = x.ReferralCode,
                                    LastSeenOn = _context.SiteReferrals
                                                         .Where(y => y.IpAddress == x.IpAddress)
                                                         .Max(y => y.Date)
                                })
                                .Distinct()
                                .OrderByDescending(x => x.LastSeenOn)
                                .ToList();

            var model = new KnownSiteUsersViewModel
            {
                KnownUsersList = users.Select(x => new SelectListItem
                {
                    Text = string.Concat(x.Name, " (Last seen ", Math.Round((DateTime.Now - x.LastSeenOn).TotalDays, 0), " days ago)"),
                    Value = x.Ip
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult index(KnownSiteUsersViewModel model)
        {
            return RedirectToAction(MVC.Reports.SiteActivity.ReferallActivity(model.SelectedUserIp));
        }

        public virtual ActionResult ReferallActivity(string ip)
        {
            var activity = _context.SiteReferrals
                                   .Where(x => x.IpAddress == ip)
                                   .OrderByDescending(x => x.Date)
                                   .Select(x => new KnownVisitorActivityViewModel.VisitorActivityViewModel
                                   {
                                       Url = x.Url,
                                       VisitDate = x.Date
                                   })
                                   .ToList();
            var model = new KnownVisitorActivityViewModel
            {
                Name = _context.SiteReferrals.Where(x => !string.IsNullOrEmpty(x.ReferralCode)).Select(x => x.ReferralCode).FirstOrDefault(),
                Activity = activity
            };

            return View(model);
        }
    }
}
