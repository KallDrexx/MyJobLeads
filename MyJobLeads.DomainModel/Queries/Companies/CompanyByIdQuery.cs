using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.DomainModel.Queries.Companies
{
    /// <summary>
    /// Queries for a company by its id value
    /// </summary>
    public class CompanyByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> _authProcess;
        protected int _companyId, _reqUserId;

        public CompanyByIdQuery(IUnitOfWork unitOfWork, IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> authProcess)
        {
            _unitOfWork = unitOfWork;
            _authProcess = authProcess;
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
        /// Specifies the id value of the user requesting the company
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CompanyByIdQuery RequestedByUserId(int userId)
        {
            _reqUserId = userId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual Company Execute()
        {
            // Make sure the user has authorization for the specified company
            if (!_authProcess.Execute(new CompanyQueryAuthorizationParams { CompanyId = _companyId, RequestingUserId = _reqUserId }).UserAuthorized)
                return null;

            // Attempt to retrieve the company
            return _unitOfWork.Companies.Fetch().Where(x => x.Id == _companyId).SingleOrDefault();
        }
    }
}
