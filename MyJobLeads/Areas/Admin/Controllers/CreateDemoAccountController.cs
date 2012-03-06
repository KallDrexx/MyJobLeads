using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Areas.Admin.Models.DemoAccountCreation;
using System.Transactions;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Infrastructure.Attributes;

namespace MyJobLeads.Areas.Admin.Controllers
{
    [RequiresSiteAdmin]
    public partial class CreateDemoAccountController : MyJobLeadsBaseController
    {
        protected CreateUserCommand _createUserCmd;

        public CreateDemoAccountController(MyJobLeadsDbContext context, CreateUserCommand createUserCmd)
        {
            _context = context;
            _createUserCmd = createUserCmd;
        }

        public virtual ActionResult Index()
        {
            return View(new CreateDemoAccountViewModel());
        }

        [HttpPost]
        public virtual ActionResult Index(CreateDemoAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var transaction = new TransactionScope())
            {
                var user = _createUserCmd.Execute(new CreateUserCommandParams
                {
                    Email = model.Email,
                    PlainTextPassword = model.Password,
                    FullName = model.Name
                });

                //_context.Users.Attach(user);
                user = _context.Users.Find(user.Id);

                // Create the organization
                var org = new MyJobLeads.DomainModel.Entities.Organization
                {
                    Name = model.OrganizationName,
                    RegistrationToken = Guid.NewGuid()
                };

                user.IsOrganizationAdmin = true;
                user.Organization = org;
                _context.Organizations.Add(org);

                // Create a new jobsearch with random data
                var jobsearch = new JobSearch();
                var values = new DemoCreationValues();
                int seed = new Random(DateTime.Now.Millisecond).Next(1000, 50000);

                foreach (var companyData in values.Companies)
                {
                    var company = new Company
                    {
                        Name = companyData[0],
                        Phone = companyData[1],
                        City = companyData[2],
                        State = companyData[3],
                        Zip = companyData[4],
                        LeadStatus = companyData[5],
                        Notes = companyData[6]
                    };

                    for (int x = 0; x < 3; x++)
                    {
                        var contactData = values.Contacts[new Random(seed++).Next(0, values.Contacts.Count - 1)];
                        var contact = new Contact
                        {
                            Name = contactData[0],
                            DirectPhone = contactData[1],
                            MobilePhone = contactData[2],
                            Extension = contactData[3],
                            Email = contactData[4],
                            Assistant = contactData[5],
                            ReferredBy = contactData[6],
                            Notes = contactData[7]
                        };

                        company.Contacts.Add(contact);
                    }

                    var task = new Task
                    {
                        Name = values.TaskNames[new Random(seed++).Next(0, values.TaskNames.Count - 1)],
                        CompletionDate = new Random(seed++).Next(0, 1000) % 2 == 0 ? DateTime.Now : (DateTime?)null,
                        TaskDate = DateTime.Now.AddDays(new Random(seed++).Next(-7, 30)).AddHours(new Random(seed++).Next(-24, 24))
                    };

                    company.Tasks.Add(task);

                    jobsearch.Companies.Add(company);
                }

                user.JobSearches.Add(jobsearch);
                _context.SaveChanges();

                // This has to be done once we have an id
                user.LastVisitedJobSearchId = jobsearch.Id;
                _context.SaveChanges();

                transaction.Complete();

                return RedirectToAction(MVC.Admin.CreateDemoAccount.Success(model.Name, model.Email, model.Password));
            }
        }

        public virtual ActionResult Success(string name, string email, string password)
        {
            return View(new CreateDemoAccountSuccessViewModel
            {
                Name = name,
                Email = email,
                Password = password
            });
        }
    }
}
