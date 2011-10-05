using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query class to retrieve a task by its id value
    /// </summary>
    public class TaskByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected IProcess<TaskAuthorizationParams, AuthorizationResultViewModel> _taskAuthProcess;
        protected int _taskId, _userId;

        public TaskByIdQuery(IUnitOfWork unitOfWork, IProcess<TaskAuthorizationParams, AuthorizationResultViewModel> taskAuthProcess)
        {
            _unitOfWork = unitOfWork;
            _taskAuthProcess = taskAuthProcess;
        }

        /// <summary>
        /// Specifies the id value of the task to retrieve
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskByIdQuery WithTaskId(int taskId)
        {
            _taskId = taskId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user running the query
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TaskByIdQuery RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public virtual Task Execute()
        {
            // Make sure the user is authorized for the task
            if (!_taskAuthProcess.Execute(new TaskAuthorizationParams { TaskId = _taskId, RequestingUserId = _userId }).UserAuthorized)
                return null;

            // Retrieve the task
            return _unitOfWork.Tasks.Fetch().Where(x => x.Id == _taskId).SingleOrDefault();
        }
    }
}
