using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Queries.Contacts
{
    /// <summary>
    /// Queries for a contact by its id value
    /// </summary>
    public class ContactByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _contactId;

        public ContactByIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public Contact Execute()
        {
            return _unitOfWork.Contacts.Fetch().Where(x => x.Id == _contactId).SingleOrDefault();
        }
    }
}
