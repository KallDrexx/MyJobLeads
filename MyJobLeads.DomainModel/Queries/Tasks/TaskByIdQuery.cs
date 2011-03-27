using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query class to retrieve a task by its id value
    /// </summary>
    public class TaskByIdQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected int _taskId;

        public TaskByIdQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public Task Execute()
        {
            // Retrieve the task
            return _unitOfWork.Tasks.Fetch().Where(x => x.Id == _taskId).SingleOrDefault();
        }
    }
}
