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
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Commands.JobSearches;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    /// <summary>
    /// Command class to create a new task for a company
    /// </summary>
    public class CreateTaskCommand
    {
        protected IServiceFactory _serviceFactory;
        protected string _name;
        protected DateTime? _taskDate;
        protected int _companyId, _userId, _contactId;
        protected string _category;

        public CreateTaskCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Specifies the id value of the company to create the task for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CreateTaskCommand WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Specifies the name of the task
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CreateTaskCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the datetime for the task
        /// </summary>
        /// <param name="taskDate"></param>
        /// <returns></returns>
        public CreateTaskCommand SetTaskDate(DateTime? taskDate)
        {
            _taskDate = taskDate;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user creating the task
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CreateTaskCommand RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the contact to associate the task with
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public CreateTaskCommand WithContactId(int contactId)
        {
            _contactId = contactId;
            return this;
        }

        /// <summary>
        /// Specifies the task's category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public CreateTaskCommand SetCategory(string category)
        {
            _category = category;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified company, calling user, or contact is not found</exception>
        public virtual Task Execute()
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the user creating the task
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the company
            var company = _serviceFactory.GetService<CompanyByIdQuery>().WithCompanyId(_companyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), _companyId);

            // Retrieve the contact if one is specified
            Contact contact = null;
            if (_contactId != 0)
            {
                contact = _serviceFactory.GetService<ContactByIdQuery>().WithContactId(_contactId).Execute();
                if (contact == null)
                    throw new MJLEntityNotFoundException(typeof(Contact), _contactId);
            }

            // Create the Task
            var task = new Task
            {
                Company = company,
                Name = _name,
                TaskDate = _taskDate,
                Contact = contact,
                Category = _category,

                History = new List<TaskHistory>()
            };

            // Create history record
            task.History.Add(new TaskHistory
            {
                Name = _name,
                TaskDate = _taskDate,
                Category = _category,
                HistoryAction = MJLConstants.HistoryInsert,
                DateModified = DateTime.Now,
                AuthoringUser = user
            });

            unitOfWork.Tasks.Add(task);
            unitOfWork.Commit();

            // Index the task with the search provider
            _serviceFactory.GetService<ISearchProvider>().Index(task);

            // Update the metrics for the job search
            _serviceFactory.GetService<UpdateJobSearchMetricsCommand>()
                .Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = (int)company.JobSearchID });

            return task;
        }
    }
}
