using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;

namespace MyJobLeads.DomainModel.Processes.Authorization
{
    public class AuthorizationProcesses 
        : IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>,
          IProcess<ContactAutorizationParams, AuthorizationResultViewModel>,
          IProcess<TaskAuthorizationParams, AuthorizationResultViewModel>,
          IProcess<PositionAuthorizationParams, AuthorizationResultViewModel>,
          IProcess<OrganizationAdminAuthorizationParams, AuthorizationResultViewModel>
    {
        public AuthorizationProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Determines if a user is authorized to to query for a company
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public AuthorizationResultViewModel Execute(CompanyQueryAuthorizationParams procParams)
        {
            return new AuthorizationResultViewModel
            {
                UserAuthorized = _context.Companies.Any(x => x.Id == procParams.CompanyId && x.JobSearch.User.Id == procParams.RequestingUserId)
            };
        }

        /// <summary>
        /// Determines if a user has access to a contact
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public AuthorizationResultViewModel Execute(ContactAutorizationParams procParams)
        {
            return new AuthorizationResultViewModel
            {
                UserAuthorized = _context.Contacts
                           .Any(x => x.Id == procParams.ContactId && x.Company.JobSearch.User.Id == procParams.RequestingUserId)
            };
        }

        /// <summary>
        /// Determines if the specified user has authorization to a specific task
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public AuthorizationResultViewModel Execute(TaskAuthorizationParams procParams)
        {
            return new AuthorizationResultViewModel
            {
                UserAuthorized = _context.Tasks
                                         .Any(x => x.Id == procParams.TaskId && x.Company.JobSearch.User.Id == procParams.RequestingUserId)
            };
        }

        /// <summary>
        /// Determines if the specified user has authorization to a specific position
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public AuthorizationResultViewModel Execute(PositionAuthorizationParams procParams)
        {
            return new AuthorizationResultViewModel
            {
                UserAuthorized = _context.Positions
                                         .Any(x => x.Id == procParams.PositionId && x.Company.JobSearch.User.Id == procParams.RequestingUserId)
            };
        }

        /// <summary>
        /// Determines if the user is authorized as an administrator for the specified organization
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public AuthorizationResultViewModel Execute(OrganizationAdminAuthorizationParams procParams)
        {
            return new AuthorizationResultViewModel
            {
                UserAuthorized = _context.Users.Any(x => x.Id == procParams.UserId && x.Organization.Id == procParams.OrganizationId && x.IsOrganizationAdmin)
            };
        }

        #region Member Variables

        protected MyJobLeadsDbContext _context;

        #endregion
    }
}
