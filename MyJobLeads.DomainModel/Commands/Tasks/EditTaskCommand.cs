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
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Commands.JobSearches;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    /// <summary>
    /// Command class that edits the specified task
    /// </summary>
    public class EditTaskCommand
    {
        protected IServiceFactory _serviceFactory;
        protected int _taskId, _userId, _contactId;
        protected DateTime? _newTaskDate;
        protected string _name, _category, _subCategory;
        protected bool _completed, _completedChanged, _dateChanged, _contactChanged;

        public EditTaskCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
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
            _contactId = contactId < 1? 0 : contactId;
            _contactChanged = true;
            return this;
        }

        /// <summary>
        /// Specifies the category for the task
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public EditTaskCommand SetCategory(string category)
        {
            _category = category;
            return this;
        }

        /// <summary>
        /// Specifies the sub-category for the task
        /// </summary>
        /// <param name="subcategory"></param>
        /// <returns></returns>
        public EditTaskCommand SetSubCategory(string subcategory)
        {
            _subCategory = subcategory;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified task, calling user, or contact isn't found</exception>
        public virtual Task Execute()
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the user editing the task
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the task
            var task = _serviceFactory.GetService<TaskByIdQuery>().WithTaskId(_taskId).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), _taskId);

            // If specified, retrieve the specified contact
            Contact contact = null;
            if (_contactChanged)
            {
                if (_contactId != 0)
                {
                    contact = _serviceFactory.GetService<ContactByIdQuery>().WithContactId(_contactId).Execute();
                    if (contact == null)
                        throw new MJLEntityNotFoundException(typeof(Contact), _contactId);
                }
            }

            // Only change the properties that were specified
            if (_name != null) { task.Name = _name; }
            if (_category != null) { task.Category = _category; }
            if (_subCategory != null) { task.SubCategory = _subCategory; }
            if (_dateChanged) { task.TaskDate = _newTaskDate; }

            if (_completedChanged) 
            {
                if (_completed)
                {
                    // Only change the completion date if the completion date isn't already set
                    if (task.CompletionDate == null)
                        task.CompletionDate = DateTime.Now;
                }
                else
                    task.CompletionDate = null;
            }

            if (_contactChanged) 
            {
                // ContactId must be set instead of Contact due to the contact not being eager loaded.
                task.ContactId = contact == null ? (int?)null : contact.Id;
            }

            // Create the history record
            task.History.Add(new TaskHistory
            {
                Name = task.Name,
                TaskDate = task.TaskDate,
                CompletionDate = task.CompletionDate,
                ContactId = task.ContactId,
                Category = task.Category,
                SubCategory = task.SubCategory,
                AuthoringUser = user,
                DateModified = DateTime.Now,
                HistoryAction = MJLConstants.HistoryUpdate
            });

            // Save
            unitOfWork.Commit();

            // Update the index for the task
            _serviceFactory.GetService<ISearchProvider>().Index(task);

            // Update the metrics for the task's job search
            var metricsCmd = _serviceFactory.GetService<UpdateJobSearchMetricsCommand>();
            metricsCmd.Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = (int)task.Company.JobSearchID });

            return task;
        }
    }
}
