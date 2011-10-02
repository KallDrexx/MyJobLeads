using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.DomainModel.Queries.Contacts
{
    /// <summary>
    /// Queries for contacts for the specified company
    /// </summary>
    public class ContactsByCompanyIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> _compAuthProcess;
        protected int _companyId, _userId;

        public ContactsByCompanyIdQuery(IUnitOfWork unitOfWork, IProcess<CompanyQueryAuthorizationParams, AuthorizationResultViewModel> compAuthProcess)
        {
            _unitOfWork = unitOfWork;
            _compAuthProcess = compAuthProcess;
        }

        /// <summary>
        /// Specifies the id value of the company to find contacts for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ContactsByCompanyIdQuery WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user performing the query
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ContactsByCompanyIdQuery RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Execute the query
        /// </summary>
        /// <returns></returns>
        public virtual IList<Contact> Execute()
        {
            // Make sure the user has access to the company
            if (!_compAuthProcess.Execute(new CompanyQueryAuthorizationParams { RequestingUserId = _userId, CompanyId = _companyId }).UserAuthorized)
                return new List<Contact>();

            return _unitOfWork.Contacts.Fetch().Where(x => x.Company.Id == _companyId).ToList();
        }
    }
}
