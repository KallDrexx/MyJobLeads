using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;

namespace MyJobLeads.DomainModel.Queries.Contacts
{
    /// <summary>
    /// Queries for a contact by its id value
    /// </summary>
    public class ContactByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected IProcess<ContactAutorizationParams, AuthorizationResultViewModel> _contactAuthProcess;
        protected int _contactId, _userId;

        public ContactByIdQuery(IUnitOfWork unitOfWork, IProcess<ContactAutorizationParams, AuthorizationResultViewModel> contactAuthProcess)
        {
            _unitOfWork = unitOfWork;
            _contactAuthProcess = contactAuthProcess;
        }

        /// <summary>
        /// Specifies the id value of the contact to retrieve
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public ContactByIdQuery WithContactId(int contactId)
        {
            _contactId = contactId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user requesting the contact
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ContactByIdQuery RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual Contact Execute()
        {
            // Check if the user is authorized for the contact
            if (!_contactAuthProcess.Execute(new ContactAutorizationParams { ContactId = _contactId, RequestingUserId = _userId }).UserAuthorized)
                return null;

            return _unitOfWork.Contacts.Fetch().Where(x => x.Id == _contactId).SingleOrDefault();
        }
    }
}
