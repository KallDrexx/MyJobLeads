using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Queries.Contacts;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    /// <summary>
    /// Command class that edits the specified task
    /// </summary>
    public class EditTaskCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected int _taskId, _userId, _contactId;
        protected DateTime? _newTaskDate;
        protected string _name;
        protected bool _completed, _completedChanged, _dateChanged, _contactChanged;

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
        /// Specifies the id value of the user editing the task
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditTaskCommand RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the contact to associate with the task
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public EditTaskCommand SetContactId(int contactId)
        {
            _contactId = contactId;
            _contactChanged = true;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified task, calling user, or contact isn't found</exception>
        public virtual Task Execute()
        {
            // Retrieve the user editing the task
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the task
            var task = new TaskByIdQuery(_unitOfWork).WithTaskId(_taskId).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), _taskId);

            // If specified, retrieve the specified contact
            Contact contact = null;
            if (_contactChanged)
            {
                if (_contactId != 0)
                {
                    contact = new ContactByIdQuery(_unitOfWork).WithContactId(_contactId).Execute();
                    if (contact == null)
                        throw new MJLEntityNotFoundException(typeof(Contact), _contactId);
                }
            }

            // Only change the properties that were specified
            if (_name != null) { task.Name = _name; }
            if (_dateChanged) { task.TaskDate = _newTaskDate; }
            if (_completedChanged) { task.Completed = _completed; }
            if (_contactChanged) { task.Contact = contact; }

            // Create the history record
            task.History.Add(new TaskHistory
            {
                Name = task.Name,
                TaskDate = task.TaskDate,
                Completed = task.Completed,
                Contact = task.Contact,
                AuthoringUser = user,
                DateModified = DateTime.Now,
                HistoryAction = MJLConstants.HistoryUpdate
            });

            // Save
            _unitOfWork.Commit();
            return task;
        }
    }
}
