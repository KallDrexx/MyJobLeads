using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Companies
{
    public class LeadStatusesAvailableForCompaniesQuery
    {
        public IList<string> Execute()
        {
            return new List<string>
            {
                MJLConstants.ProspectiveEmployerCompanyStatus,
                MJLConstants.InterviewingCompanyStatus,
                MJLConstants.DeadLeadCompanyStatus
            }
                .OrderBy(x => x)
                .ToList();
        }
    }
}
