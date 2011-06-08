using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers;

namespace MyJobLeads.DomainModel.Queries.Companies
{
    /// <summary>
    /// Query that retrieves all companies that are part of a job search
    /// </summary>
    public class CompaniesByJobSearchIdQuery
    {
        protected IServiceFactory _serviceFactory;
        protected int _searchId;
        protected bool _sortByName;

        public CompaniesByJobSearchIdQuery(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        /// <summary>
        /// Specifies the id value of the job search to find companies for
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public CompaniesByJobSearchIdQuery WithJobSearchId(int jobSearchId)
        {
            _searchId = jobSearchId;
            return this;
        }

        /// <summary>
        /// Specifies to sort results by company name
        /// </summary>
        /// <returns></returns>
        public CompaniesByJobSearchIdQuery SortByName()
        {
            _sortByName = true;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual IList<Company> Execute()
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            var query = unitOfWork.Companies.Fetch().Where(x => x.JobSearchID == _searchId);

            if (_sortByName)
                query = query.OrderBy(x => x.Name);

            return query.ToList();
        }
    }
}
