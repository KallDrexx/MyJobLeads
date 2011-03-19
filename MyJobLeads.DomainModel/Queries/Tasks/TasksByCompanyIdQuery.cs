using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query class for retrieving tasks associated with a company
    /// </summary>
    public class TasksByCompanyIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _companyId;

        public TasksByCompanyIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the company to retrieve tasks for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public TasksByCompanyIdQuery WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        public IList<Task> Execute()
        {
            return _unitOfWork.Tasks.Fetch().Where(x => x.Company.Id == _companyId).ToList();
        }
    }
}
