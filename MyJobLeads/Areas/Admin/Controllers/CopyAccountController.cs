using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.Areas.Admin.Models;
using MyJobLeads.DomainModel.Commands.Users;
using System.Transactions;
using AutoMapper;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.Admin.Controllers
{
    [RequiresSiteAdmin]
    public partial class CopyAccountController : MyJobLeadsBaseController
    {
        protected CreateUserCommand _createUserCmd;

        public CopyAccountController(MyJobLeadsDbContext context, CreateUserCommand createUserCmd)
        {
            _context = context;
            _createUserCmd = createUserCmd;
        }

        public virtual ActionResult Index()
        {
            var users = _context.Users.ToList();
            var model = new CopyAccountDataViewModel();
            model.SetAccountList(users);
            
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(CopyAccountDataViewModel model)
        {
            using (var transaction = new TransactionScope())
            {
                var user = _createUserCmd.Execute(new CreateUserCommandParams
                {
                    Email = model.NewUsername,
                    PlainTextPassword = model.Password,
                    FullName = "Demo Account"
                });

                _context.Users.Attach(user);

                if (model.CreateAsOrgAdmin)
                {
                    var org = new MyJobLeads.DomainModel.Entities.Organization
                    {
                        Name = "New Organization",
                        RegistrationToken = Guid.NewGuid()
                    };

                    user.IsOrganizationAdmin = true;
                    user.Organization = org;
                    _context.Organizations.Add(org);
                }
                
                // Copy the old user's data to the new user
                var oldUser = _context.Users.Where(x => x.Id == model.SelectedAccountId).Single();

                var jobSearch = new JobSearch
                {
                    Name = user.LastVisitedJobSearch.Name,
                    Description = user.LastVisitedJobSearch.Description,
                    CurrentMilestone = user.LastVisitedJobSearch.CurrentMilestone,
                    Id = 0,
                    UserId = user.Id,
                    User = user,
                    Companies = user.LastVisitedJobSearch.Companies.Select(company => new Company
                    {
                        Id = 0,
                        JobSearchID = null,
                        JobSearch = null,
                        City = company.City,
                        Industry = company.Industry,
                        LeadStatus = company.LeadStatus,
                        MetroArea = company.MetroArea,
                        Name = company.Name,
                        Notes = company.Notes,
                        Phone = company.Phone,
                        State = company.State,
                        Zip = company.Zip
                    }).ToList()
                };


                var jobSearch = Mapper.Map<JobSearch, JobSearch>(oldUser.LastVisitedJobSearch);
                user.LastVisitedJobSearch = jobSearch;
                user.JobSearches.Add(jobSearch);

                //_context.Entry(user).State = System.Data.EntityState.Modified;
                _context.SaveChanges();
                transaction.Complete();

                return RedirectToAction(MVC.Admin.CopyAccount.CopyAccountSuccess(user.Email, model.Password));
            }
        }

        public virtual ActionResult CopyAccountSuccess(string newUserName, string password)
        {
            var model = new CopyAccountSuccessfulViewModel
            {
                UserName = newUserName,
                Password = password
            };

            return View(model);
        }
    }
}
