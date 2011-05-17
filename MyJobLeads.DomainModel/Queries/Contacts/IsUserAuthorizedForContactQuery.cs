using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Contacts
{
    /// <summary>
    /// Query that determines if the specified user is authorized to access the specified contact
    /// </summary>
    public class IsUserAuthorizedForContactQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId, _contactId;

        public IsUserAuthorizedForContactQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the user to check authorization for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForContactQuery WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the contact to check authorization for
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForContactQuery WithContactId(int contactId)
        {
            _contactId = contactId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            // Determine if the specified user created the jobsearch the contact is associated with
            return _unitOfWork.Contacts.Fetch().Any(x => x.Id == _contactId && x.JobSearch.User.Id == _userId);
        }
    }
}
