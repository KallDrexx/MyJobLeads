using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query that determines if a user is authorized for access to a task
    /// </summary>
    public class IsUserAuthorizedForTaskQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _userId, _taskId;

        public IsUserAuthorizedForTaskQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the user to check authorization for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForTaskQuery WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the task to check authorization for
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IsUserAuthorizedForTaskQuery WithTaskId(int taskId)
        {
            _taskId = taskId;
            return this;
        }

        /// <summary>
        /// Executes the result
        /// </summary>
        /// <returns>Returns true if the specified user is authorized for access to the task</returns>
        public bool Execute()
        {
            // Check if the task's associated job search belongs to the user
            return _unitOfWork.Tasks.Fetch().Any(x => x.Id == _taskId && x.Company.JobSearch.User.Id == _userId);
        }
    }
}
