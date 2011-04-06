using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    /// <summary>
    /// Command class to create a new task for a company
    /// </summary>
    public class CreateTaskForCompanyCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected string _name;
        protected DateTime? _taskDate;
        protected int _companyId, _userId;

        public CreateTaskForCompanyCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Specifies the id value of the company to create the task for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CreateTaskForCompanyCommand WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Specifies the name of the task
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CreateTaskForCompanyCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the datetime for the task
        /// </summary>
        /// <param name="taskDate"></param>
        /// <returns></returns>
        public CreateTaskForCompanyCommand SetTaskDate(DateTime? taskDate)
        {
            _taskDate = taskDate;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user creating the task
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CreateTaskForCompanyCommand RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified company or calling user is not found</exception>
        public Task Execute()
        {
            // Retrieve the user creating the task
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the company
            var company = new CompanyByIdQuery(_unitOfWork).WithCompanyId(_companyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), _companyId);

            // Create the Task
            var task = new Task
            {
                Company = company,
                Name = _name,
                TaskDate = _taskDate,

                History = new List<TaskHistory>()
            };

            // Create history record
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
