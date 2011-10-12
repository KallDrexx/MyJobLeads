using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Organizations;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.ProcessParams.Users;

namespace MyJobLeads.DomainModel.Processes.Organizations
{
    public class OrganizationDocumentProcesses : IProcess<OrgMemberDocVisibilityByOrgAdminParams, GeneralSuccessResultViewModel>,
                                                 IProcess<ByUserIdParams, VisibleOrgOfficialDocListForUserViewModel>
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

        /// <summary>
        /// Retrieves a list of all documents the user is eligible to see
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public VisibleOrgOfficialDocListForUserViewModel Execute(ByUserIdParams procParams)
        {
            var user = _context.Users
                               .Where(x => x.Id == procParams.UserId)
                               .Include(x => x.Organization.MemberOfficialDocuments)
                               .SingleOrDefault();

            if (user == null || user.Organization == null)
            {
                return new VisibleOrgOfficialDocListForUserViewModel
                {
                    UserVisibleDocuments = new List<OfficialDocument>()
                };
            }

            return new VisibleOrgOfficialDocListForUserViewModel
            {
                UserVisibleDocuments = user.Organization.MemberOfficialDocuments.ToList()
            };      
        }
    }
}
