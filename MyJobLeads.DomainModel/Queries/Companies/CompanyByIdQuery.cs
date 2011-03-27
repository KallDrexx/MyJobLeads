using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Queries.Companies
{
    /// <summary>
    /// Queries for a company by its id value
    /// </summary>
    public class CompanyByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _companyId;

        public CompanyByIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the company to retrieve
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CompanyByIdQuery WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public Company Execute()
        {
            // Attempt to retrieve the company
            return _unitOfWork.Companies.Fetch().Where(x => x.Id == _companyId).SingleOrDefault();
        }
    }
}
