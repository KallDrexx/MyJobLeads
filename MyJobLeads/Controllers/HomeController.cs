﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.ViewModels.Users;

namespace MyJobLeads.Controllers
{
    public enum ActiveSidebarLink { None, Logon, Register, Tasks, Companies, Positions, About, OrgAdmin };

    public partial class HomeController : MyJobLeadsBaseController
    {
        public HomeController(UserByIdQuery userByIdQuery, IServiceFactory factory)
        {
            _userByIdQuery = userByIdQuery;
            _serviceFactory = factory;
            _unitOfWork = factory.GetService<IUnitOfWork>();
        }

        private UserByIdQuery _userByIdQuery;

        public virtual ActionResult Index()
        {
            if (CurrentUserId != 0)
            {
                var user = _userByIdQuery.WithUserId(CurrentUserId).Execute();

                if (user.LastVisitedJobSearchId == null)
                    return RedirectToAction(MVC.JobSearch.Add());

                return RedirectToAction(MVC.Task.Index());
            }

            
            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult SubmitFeedback(string name, string email, string feedback, string fromUrl)
        {
            feedback = HttpUtility.HtmlDecode(feedback);

            string emailBody = string.Format(
@"The following feedback was submitted:

Name: {0}
Email: {1}
URL: {2}
Date: {3}

Feedback: 
{4}",
                name,
                email,
                fromUrl,
                DateTime.Now.ToString(),
                feedback);

            EmailUtils emailUtil = new EmailUtils();
            emailUtil.Send("feedback@interviewtools.net", "Feedback Submitted", emailBody);

            return View();
        }

        public virtual ActionResult FixBlankTitles()
        {
            const string BLANK = "No Name";
            int fixedCompanies = 0, fixedContacts = 0, fixedTasks = 0;

            var companies = _unitOfWork.Companies.Fetch().Where(x => x.Name.Trim() == string.Empty).ToList();
            foreach (var comp in companies)
            {
                fixedCompanies++;
                comp.Name = BLANK;
            }

            var contacts = _unitOfWork.Contacts.Fetch().Where(x => x.Name.Trim() == string.Empty).ToList();
            foreach (var cont in contacts)
            {
                fixedContacts++;
                cont.Name = BLANK;
            }

            var tasks = _unitOfWork.Tasks.Fetch().Where(x => x.Name.Trim() == string.Empty).ToList();
            foreach (var task in tasks)
            {
                fixedTasks++;
                task.Name = BLANK;
            }

            _unitOfWork.Commit();

            return View(fixedCompanies + fixedContacts + fixedTasks);
        }

        public virtual ActionResult SidebarDisplay(ActiveSidebarLink activeLink)
        {
            if (CurrentUserId != 0)
            {
                var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
                var model = new UserSidebarViewModel(user) { ActiveLink = activeLink };
                return PartialView(MVC.Home.Views._LoggedInSidebarDisplay, model);
            }

            else
            {
                return PartialView(MVC.Home.Views._AnonymousUserSidebarDisplay, activeLink);
            }
        }
    }
}
