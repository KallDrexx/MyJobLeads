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

            foreach (var user in (IList<User>)context.SourceValue)
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
