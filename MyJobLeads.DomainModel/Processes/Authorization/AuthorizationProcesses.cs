﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;

namespace MyJobLeads.DomainModel.Processes.Authorization
{
    public class AuthorizationProcesses : IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel>
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

        #region Member Variables

        protected MyJobLeadsDbContext _context;

        #endregion
    }
}
