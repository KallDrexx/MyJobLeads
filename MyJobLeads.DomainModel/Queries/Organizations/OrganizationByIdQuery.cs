using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers;

namespace MyJobLeads.DomainModel.Queries.Organizations
{
    public struct OrganizationByIdQueryParams
    {
        public int OrganizationId { get; set; }
    }

    public class OrganizationByIdQuery
    {
        protected IServiceFactory _serviceFactory;

        public OrganizationByIdQuery(IServiceFactory iServiceFactory)
        {
            _serviceFactory = iServiceFactory;
        }

        public Organization Execute(OrganizationByIdQueryParams queryParams)
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            return unitOfWork.Organizations.Fetch().Where(x => x.Id == queryParams.OrganizationId).SingleOrDefault();
        }
    }
}
