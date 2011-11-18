using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Users;
using MyJobLeads.DomainModel.ProcessParams.Users;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Processes.Admin
{
    public class OrganizationAdminProcesses : IProcess<CreateOrgWithAdminUserParams, GeneralSuccessResultViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel> _generatePasswordProcess;
        protected IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> _siteAdminAuthProcess;

        public OrganizationAdminProcesses(MyJobLeadsDbContext context, 
                                          IProcess<GenerateUserPasswordHashParams, GeneratedPasswordHashViewModel> genPasswordProcess,
                                          IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> siteAdminAuthProcess)
        {
            _context = context;
            _generatePasswordProcess = genPasswordProcess;
            _siteAdminAuthProcess = siteAdminAuthProcess;
        }

        /// <summary>
        /// Creates a new organization with an organization administrator
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(CreateOrgWithAdminUserParams procParams)
        {
            var authResult = _siteAdminAuthProcess.Execute(new SiteAdminAuthorizationParams { UserId = procParams.RequestingUserId });
            if (!authResult.UserAuthorized)
                throw new UserNotAuthorizedForProcessException(procParams.RequestingUserId, typeof(SiteAdminAuthorizationParams), typeof(AuthorizationResultViewModel));

            string hashedPassword = _generatePasswordProcess.Execute(
                new GenerateUserPasswordHashParams { Email = procParams.AdminEmail, PlainTextPassword = procParams.AdminPlainTextPassword }).PasswordHash;

            // Create the organization and user structures
            var org = new Organization { Name = procParams.OrganizationName };
            org.Members.Add(new User
            {
                Email = procParams.AdminEmail,
                Password = hashedPassword,
                FullName = procParams.AdminName,
                IsOrganizationAdmin = true
            });

            _context.Organizations.Add(org);
            _context.SaveChanges();

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }
    }
}
