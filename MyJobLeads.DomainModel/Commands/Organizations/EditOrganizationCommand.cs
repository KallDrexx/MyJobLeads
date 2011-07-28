using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.Configuration;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Commands.Organizations
{
    public struct EditOrganizationCommandParams
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string EmailDomainRestriction { get; set; }

        public bool? IsEmailDomainRestrictedValue { get; private set; }
        public bool IsEmailDomainRestricted { set { IsEmailDomainRestrictedValue = value; } }
    }

    public class EditOrganizationCommand
    {
        protected IServiceFactory _serviceFactory;

        public EditOrganizationCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        public Organization Execute(EditOrganizationCommandParams cmdParams)
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the organization
            var org = unitOfWork.Organizations.Fetch().Where(x => x.Id == cmdParams.OrganizationId).SingleOrDefault();
            if (org == null)
                throw new MJLEntityNotFoundException(typeof(Organization), cmdParams.OrganizationId);
            
            // Edit the organization's details
            if (cmdParams.Name != null) { org.Name = cmdParams.Name; }
            if (cmdParams.IsEmailDomainRestrictedValue != null) { org.IsEmailDomainRestricted = (bool)cmdParams.IsEmailDomainRestrictedValue; }

            if (cmdParams.EmailDomainRestriction != null)
            {
                org.EmailDomains.Clear();
                org.EmailDomains.Add(new OrganizationEmailDomain { Domain = cmdParams.EmailDomainRestriction });
            }

            // Save the organization
            unitOfWork.Commit();

            return org;
        }
    }
}
