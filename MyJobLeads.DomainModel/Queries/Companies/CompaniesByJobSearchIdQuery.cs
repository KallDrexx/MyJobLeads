using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Companies
{
    /// <summary>
    /// Query that retrieves all companies that are part of a job search
    /// </summary>
    public class CompaniesByJobSearchIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _searchId;

        public CompaniesByJobSearchIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual IList<Company> Execute()
        {
            return _unitOfWork.Companies.Fetch().Where(x => x.JobSearch.Id == _searchId).ToList();
        }
    }
}
