using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.FillPerfect;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.Ordering;

namespace MyJobLeads.DomainModel.QueryExtensions
{
    public static class UserOrderQueries
    {
        public static IQueryable<FpUserLicense> UserActivatedLicenses(this IQueryable<FpUserLicense> query, int userId)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            return query.Where(x => x.Order.OrderedForId == userId && x.Order.OrderStatusValue == (int)OrderStatus.Completed);
        }

        public static IQueryable<Order> UserCompletedOrders(this IQueryable<Order> query, int userId)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            return query.Where(x => x.OrderedForId == userId && x.OrderStatusValue == (int)OrderStatus.Completed);
        }

        public static IQueryable<FpUserLicense> UserActiveLicense(this IQueryable<Order> query, int userId)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            return query.Where(x => x.OrderedForId == userId)
                        .SelectMany(x => x.FillPerfectLicenses)
                        .Where(x => x.EffectiveDate <= DateTime.Today && x.ExpirationDate > DateTime.Today);
        }

        public static DateTime UserLatestFillPerfectActivatedLicenseExpirationDate(this IQueryable<FpUserLicense> query, int userId)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            return query.UserActivatedLicenses(userId)
                        .Select(x => x.ExpirationDate)
                        .DefaultIfEmpty()
                        .Max();
        }
    }
}
