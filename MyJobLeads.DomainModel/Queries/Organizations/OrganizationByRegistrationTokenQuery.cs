using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads.DomainModel.Queries.Organizations
{
    public struct OrganizationByRegistrationTokenQueryParams
    {
        public Guid RegistrationToken { get; set; }
    }

    /// <summary>
    /// Queries for an organization with the specified registration token
    /// </summary>
    public class OrganizationByRegistrationTokenQuery
    {
        protected IServiceFactory _serviceFactory;

        public OrganizationByRegistrationTokenQuery(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <param name="cmdParams"></param>
        /// <returns>Returns the specified organization, or null if no organization exists with the specified registration token</returns>
        public virtual Organization Execute(OrganizationByRegistrationTokenQueryParams cmdParams)
        {
            return _serviceFactory.GetService<IUnitOfWork>().Organizations.Fetch()
                                    .Where(x => x.RegistrationToken == cmdParams.RegistrationToken).FirstOrDefault();
        }
    }
}
