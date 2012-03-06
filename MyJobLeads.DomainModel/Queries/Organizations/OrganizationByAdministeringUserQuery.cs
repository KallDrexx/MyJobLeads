using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using MyJobLeads.DomainModel.Entities.EF;

namespace MyJobLeads.DomainModel.Queries.Organizations
{
    public struct OrganizationByAdministeringUserQueryParams
    {
        public int AdministeringUserId { get; set; }
    }

    public class OrganizationByAdministeringUserQuery
    {
        protected MyJobLeadsDbContext _context;

        public OrganizationByAdministeringUserQuery (MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public OrganizationDashboardViewModel Execute(OrganizationByAdministeringUserQueryParams queryParams)
        {
            var org = _context.Users
                              .Where(x => x.Id == queryParams.AdministeringUserId)
                              .Where(x => x.IsOrganizationAdmin)
                              .Select(x => x.Organization)
                              .FirstOrDefault();

            if (org == null)
                return null;

            return new OrganizationDashboardViewModel
            {
                Organization = org,
                NonMemberOfficialDocuments = _context.OfficialDocuments.Where(x => !x.MeantForMembers).ToList(),
                HiddenMemberDocuments = _context.OfficialDocuments.Where(x => x.MeantForMembers && !x.Organizations.Any(y =>y.Id == org.Id)).ToList(),
                NumMembers = org.Members.Count,
                NumCompanies = _context.Companies.Where(x => x.JobSearch.User.Organization.Id == org.Id).Count(),
                NumContacts = _context.Contacts.Where(x => x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                NumOpenPhoneTasks = _context.Tasks.Where(x => x.Category == MJLConstants.PhoneInterviewTaskCategory && x.CompletionDate == null
                                                            && x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                NumClosedPhoneTasks = _context.Tasks.Where(x => x.Category == MJLConstants.PhoneInterviewTaskCategory && x.CompletionDate != null
                                                            && x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                NumOpenInterviewTasks = _context.Tasks.Where(x => x.Category == MJLConstants.InPersonInterviewTaskCategory && x.CompletionDate == null
                                                            && x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                NumClosedInterviewTasks = _context.Tasks.Where(x => x.Category == MJLConstants.InPersonInterviewTaskCategory && x.CompletionDate != null
                                                            && x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                NumAppliedPositions = _context.Positions.Where(x => x.HasApplied && x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                NumNotAppliedPositions = _context.Positions.Where(x => !x.HasApplied && x.Company.JobSearch.User.Organization.Id == org.Id).Count(),
                IsInFillPerfectPilot = !string.IsNullOrWhiteSpace(org.FillPerfectPilotKey)
            };
        }
    }
}
