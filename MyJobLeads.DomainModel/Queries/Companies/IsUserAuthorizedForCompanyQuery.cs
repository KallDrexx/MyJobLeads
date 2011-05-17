using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads.DomainModel.Queries.Companies
{
    /// <summary>
    /// Query that determines if the specified user is authorized for the specified company
    /// </summary>
    public class IsUserAuthorizedForCompanyQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId, _companyId;

        public IsUserAuthorizedForCompanyQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the user to check authorization for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForCompanyQuery WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the company to check a user's authorization for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForCompanyQuery WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns>Returns true if the user is authorized for access to the specified company, false if they are not</returns>
        public bool Execute()
        {
            // Check if the company's job search belongs to the specified user
            return _unitOfWork.Companies.Fetch().Count(x => x.Id == _companyId && x.JobSearch.UserId == _userId) > 0;
        }
    }
}
