using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Processes.Organizations
{
    public class OrganizationDocumentProcesses : IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public OrganizationDocumentProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sets an official document as visible to organization members
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(OrgMemberDocVisibilityByOrgAdminParams procParams)
        {
            var user = _context.Users.Where(x => x.Id == procParams.RequestingUserId).FirstOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            if (!user.IsOrganizationAdmin)
                throw new UserNotAuthorizedForEntityException(typeof(Organization), Convert.ToInt32(user.OrganizationId), user.Id);

            var doc = _context.OfficialDocuments.Where(x => x.Id == procParams.OfficialDocumentId).FirstOrDefault();
            if (procParams.ShowToMembers)
                user.Organization.MemberOfficialDocuments.Add(doc);
            else
                user.Organization.MemberOfficialDocuments.Remove(doc);

            _context.SaveChanges();

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }
    }
}
