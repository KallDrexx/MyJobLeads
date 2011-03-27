using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    /// <summary>
    /// Command class that edits the specified task
    /// </summary>
    public class EditTaskCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected int _taskId;
        protected DateTime? _newTaskDate;
        protected string _name;
        protected bool _completed, _completedChanged, _dateChanged;

        public EditTaskCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the task to edit
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public EditTaskCommand WithTaskId(int taskId)
        {
            _taskId = taskId;
            return this;
        }

        /// <summary>
        /// Specifies the new name for the task
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EditTaskCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the date for the task
        /// </summary>
        /// <param name="taskDate"></param>
        /// <returns></returns>
        public EditTaskCommand SetTaskDate(DateTime? taskDate)
        {
            _dateChanged = true;
            _newTaskDate = taskDate;
            return this;
        }

        /// <summary>
        /// Specifies the completed status of the task
        /// </summary>
        /// <param name="completedStatus"></param>
        /// <returns></returns>
        public EditTaskCommand SetCompleted(bool completedStatus)
        {
            _completedChanged = true;
            _completed = completedStatus;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified task isn't found</exception>
        public Task Execute()
        {
            // Retrieve the task
            var task = new TaskByIdQuery(_unitOfWork).WithTaskId(_taskId).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), _taskId);

            // Only change the properties that were specified
            if (_name != null) { task.Name = _name; }
            if (_dateChanged) { task.TaskDate = _newTaskDate; }
            if (_completedChanged) { task.Completed = _completed; }

            // Save
            _unitOfWork.Commit();
            return task;
        }
    }
}
