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

namespace MyJobLeads.Areas.Admin.Controllers
{
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

                _context.Users.Attach(user);

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
                        var contactData = values.Contacts[new Random().Next(0, values.Contacts.Count - 1)];
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

                        var hasTask = Convert.ToBoolean(new Random().Next(0, 1));
                        if (hasTask)
                        {
                            var random = new Random();
                            var task = new Task
                            {
                                Name = values.TaskNames[random.Next(0, values.TaskNames.Count - 1)],
                                CompletionDate = random.Next(0, 1) == 1 ? DateTime.Now : (DateTime?)null,
                                TaskDate = random.Next(0, 1) == 1
                                    ? DateTime.Now.AddDays(random.Next(-7, 14))
                                    : (DateTime?)null
                            };

                            contact.Tasks.Add(task);
                        }

                        company.Contacts.Add(contact);
                    }

                    var hasCompanyTask = Convert.ToBoolean(new Random().Next(0, 1));
                    if (hasCompanyTask)
                    {
                        var random = new Random();
                        var task = new Task
                        {
                            Name = values.TaskNames[random.Next(0, values.TaskNames.Count - 1)],
                            CompletionDate = random.Next(0, 1) == 1 ? DateTime.Now : (DateTime?)null,
                            TaskDate = random.Next(0, 1) == 1
                                ? DateTime.Now.AddDays(random.Next(-7, 14))
                                : (DateTime?)null
                        };

                        company.Tasks.Add(task);
                    }

                    jobsearch.Companies.Add(company);
                }

                jobsearch.User = user;
                user.LastVisitedJobSearch = jobsearch;

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
