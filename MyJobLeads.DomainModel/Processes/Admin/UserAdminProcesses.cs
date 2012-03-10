using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Data.Sample;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Admin;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.DomainModel.Processes.Admin
{
    public class UserAdminProcesses : IProcess<CreateDemoAccountParams, CreatedDemoAccountViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> _siteAdminAuthProc;
        protected CreateUserCommand _createUserCmd;

        public UserAdminProcesses(MyJobLeadsDbContext context,
                                    IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> siteAdminAuthProc,
                                    CreateUserCommand createUserCmd)
        {
            _context = context;
            _siteAdminAuthProc = siteAdminAuthProc;
            _createUserCmd = createUserCmd;
        }

        /// <summary>
        /// Creates a new demo account
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public CreatedDemoAccountViewModel Execute(CreateDemoAccountParams procParams)
        {
            if (!_siteAdminAuthProc.Execute(new SiteAdminAuthorizationParams { UserId = procParams.RequestingUserID }).UserAuthorized)
                throw new UserNotAuthorizedForProcessException(procParams.RequestingUserID, typeof(CreateDemoAccountParams), typeof(CreatedDemoAccountViewModel));

            int userId = 0;

            using (var transaction = new TransactionScope())
            {
                var user = _createUserCmd.Execute(new CreateUserCommandParams
                {
                    Email = procParams.Email,
                    PlainTextPassword = procParams.Password,
                    FullName = procParams.Name
                });

                //_context.Users.Attach(user);
                user = _context.Users.Find(user.Id);

                // Create the organization
                var org = new MyJobLeads.DomainModel.Entities.Organization
                {
                    Name = procParams.OrganizationName,
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
            }

            return new CreatedDemoAccountViewModel
            {
                NewUserId = userId,
                Name = procParams.Name,
                Email = procParams.Email,
                Password = procParams.Password
            };
        }
    }
}
