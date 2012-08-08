using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels.Organizations;
using AutoMapper;
using MyJobLeads.DomainModel.Entities.Metrics;

namespace MyJobLeads.DomainModel.EntityMapping.Mappings.CustomConverters
{
    public class OrganizationMemberStatisticsViewModelConverter : ITypeConverter<IList<User>, OrganizationMemberStatisticsViewModel>
    {
        public OrganizationMemberStatisticsViewModel Convert(ResolutionContext context)
        {
            var result = (OrganizationMemberStatisticsViewModel)context.DestinationValue ?? new OrganizationMemberStatisticsViewModel();
            result.MemberStats = new List<OrganizationMemberStatisticsViewModel.MemberStatistic>();

            var users = ((IList<User>)context.SourceValue).Where(x => x.LastVisitedJobSearchId != null);

            foreach (var user in users)
            {
                var stat = new OrganizationMemberStatisticsViewModel.MemberStatistic
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Metrics = user.LastVisitedJobSearch.Metrics
                };

                result.MemberStats.Add(stat);
            }

            return result;
        }
    }
}
