using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query that retrieves tasks associated with a contact
    /// </summary>
    public class TasksByContactIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _contactId;

        public TasksByContactIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the Contact to retrieve tasks for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public TasksByContactIdQuery WithContactId(int contactId)
        {
            _contactId = contactId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        public virtual IList<Task> Execute()
        {
            return _unitOfWork.Tasks.Fetch().Where(x => x.Contact.Id == _contactId).ToList();
        }
    }
}
