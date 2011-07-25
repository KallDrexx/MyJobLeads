using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

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

        public Organization Execute(OrganizationByAdministeringUserQueryParams queryParams)
        {
            var unitofwork = _serviceFactory.GetService<IUnitOfWork>();
            return unitofwork.Users
                             .Fetch()
                             .Where(x => x.Id == queryParams.AdministeringUserId)
                             .Where(x => x.IsOrganizationAdmin)
                             .Select(x => x.Organization)
                             .FirstOrDefault();
        }
    }
}
