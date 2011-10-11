using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Organizations;

namespace MyJobLeads.DomainModel.Queries.Organizations
{
    public struct OrganizationByAdministeringUserQueryParams
    {
        public int AdministeringUserId { get; set; }
    }

    public class OrganizationByAdministeringUserQuery
    {
        protected IServiceFactory _serviceFactory;

        public OrganizationByAdministeringUserQuery (IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        public OrganizationDashboardViewModel Execute(OrganizationByAdministeringUserQueryParams queryParams)
        {
            var unitofwork = _serviceFactory.GetService<IUnitOfWork>();
            return new OrganizationDashboardViewModel
            {
                Organization = unitofwork.Users
                             .Fetch()
                             .Where(x => x.Id == queryParams.AdministeringUserId)
                             .Where(x => x.IsOrganizationAdmin)
                             .Select(x => x.Organization)
                             .FirstOrDefault(),

                NonMemberOfficialDocuments = unitofwork.OfficialDocuments
                                                       .Fetch()
                                                       .Where(x => !x.MeantForMembers)
                                                       .ToList()
            };
        }
    }
}
