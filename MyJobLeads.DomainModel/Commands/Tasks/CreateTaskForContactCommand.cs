using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    /// <summary>
    /// Command class that creates a task for a contact
    /// </summary>
    public class CreateTaskForContactCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected string _name;
        protected DateTime? _taskDate;
        protected int _contactId, _userId;

        public CreateTaskForContactCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the contact to create the task for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CreateTaskForContactCommand WithContactId(int companyId)
        {
            _contactId = companyId;
            return this;
        }

        /// <summary>
        /// Specifies the name of the task
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CreateTaskForContactCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the datetime for the task
        /// </summary>
        /// <param name="taskDate"></param>
        /// <returns></returns>
        public CreateTaskForContactCommand SetTaskDate(DateTime? taskDate)
        {
            _taskDate = taskDate;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user creating the task
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CreateTaskForContactCommand RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified contact or calling user is not found</exception>
        public Task Execute()
        {
            // Retrieve the user creating the task
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the company
            var contact = new ContactByIdQuery(_unitOfWork).WithContactId(_contactId).Execute();
            if (contact == null)
                throw new MJLEntityNotFoundException(typeof(Contact), _contactId);

            // Create the Task
            var task = new Task
            {
                Contact = contact,
                Name = _name,
                TaskDate = _taskDate,

                History = new List<TaskHistory>()
            };

            // Create the history record
            task.History.Add(new TaskHistory
            {
                Name = _name,
                TaskDate = _taskDate,

                HistoryAction = MJLConstants.HistoryInsert,
                DateModified = DateTime.Now,
                AuthoringUser = user
            });

            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Commit();

            return task;
        }
    }
}
