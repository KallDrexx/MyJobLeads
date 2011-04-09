using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Contacts
{
    /// <summary>
    /// Queries for contacts for the specified company
    /// </summary>
    public class ContactsByCompanyIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _companyId;

        public ContactsByCompanyIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        /// Execute the query
        /// </summary>
        /// <returns></returns>
        public virtual IList<Contact> Execute()
        {
            return _unitOfWork.Contacts.Fetch().Where(x => x.Company.Id == _companyId).ToList();
        }
    }
}
