using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;

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
        protected int _contactId;

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
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified company is not found</exception>
        public Task Execute()
        {
            // Retrieve the company
            var company = new ContactByIdQuery(_unitOfWork).WithContactId(_contactId).Execute();

            // Create the Task
            var task = new Task
            {
                Contact = company,
                Name = _name,
                TaskDate = _taskDate
            };

            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Commit();

            return task;
        }
    }
}
